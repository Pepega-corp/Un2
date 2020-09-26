using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Entities;
using Unicon.Common.Result;
using UpdateDeviceDefinitionCommand = Unicon.Common.Commands.UpdateDeviceDefinitionCommand;

namespace Unicon.Backend.WebApi.Handlers.Command
{
    public class UpdateDeviceDefinitionCommandHandler:HandlerBase<UpdateDeviceDefinitionCommand,Result<int>>
    {
      
        public override async Task<Result<int>> HandleTyped(UpdateDeviceDefinitionCommand input,
	        ApplicationContext applicationContext)
        {
            var lastStoreEntry=await applicationContext.CommandsStoreEntries.OrderByDescending(entry => entry.Id).FirstOrDefaultAsync();

            var sequenceNumber = lastStoreEntry == null ? 0 : lastStoreEntry.SequenceNumber + 1;
            var command =
                applicationContext.UpdateDeviceDefinitionCommands.Add(new Entities.UpdateDeviceDefinitionCommand()
                {
                    DefinitionString = input.DefinitionString,
                    TagsOneLine =input.Tags!=null? string.Join("&",input.Tags):null
                });
            applicationContext.CommandsStoreEntries.Add(new CommandsStoreEntry()
            {
                CommandType = nameof(UpdateDeviceDefinitionCommand),
                SequenceNumber = sequenceNumber,
                CommandDateTime = DateTime.Now,
                RelatedCommandId = command.Entity.Id
            });
            await applicationContext.SaveChangesAsync();
            if (input.DeviceDefinitionId.HasValue)
            {
                DeviceDefinition deviceDefinition =
                    await applicationContext.DeviceDefinitions.FirstOrDefaultAsync(definition =>
                        definition.Id == input.DeviceDefinitionId);

                return await FillDeviceDefinition(input, deviceDefinition, applicationContext);
            }
            else
            {
                DeviceDefinition deviceDefinition = (await applicationContext.DeviceDefinitions.AddAsync(new DeviceDefinition())).Entity;
                return await FillDeviceDefinition(input, deviceDefinition, applicationContext);
            }
            return Result<int>.FailResult();  
        }

        private async Task<Result<int>> FillDeviceDefinition(UpdateDeviceDefinitionCommand input,
	        DeviceDefinition deviceDefinition, ApplicationContext applicationContext)
        {
            if (input.Tags != null)
            {
                var relatedTags= await applicationContext.Tags.Where(tag => input.Tags.Contains(tag.Name)).ToListAsync();

                relatedTags.ForEach(tag =>
                {
                    if (tag.RelatedDeviceDefinitions.All(definition => definition.Id != deviceDefinition.Id))
                    {
                        tag.RelatedDeviceDefinitions.Add(deviceDefinition);
                    }
                });
                
                var notExistingTags = input.Tags.Where(tagName => relatedTags.All(tag => tag.Name != tagName)).ToList();
                await applicationContext.Tags.AddRangeAsync(notExistingTags.Select(notExistingTag => new Tag()
                {
                    Name = notExistingTag,
                    RelatedDeviceDefinitions = new List<DeviceDefinition>() {deviceDefinition}
                }));
               
            }

            if (input.DefinitionString != null)
            {
                deviceDefinition.DefinitionString = input.DefinitionString;
            }
            await applicationContext.SaveChangesAsync();
            return Result<int>.SuccessResult(deviceDefinition.Id);
        }
    }
}