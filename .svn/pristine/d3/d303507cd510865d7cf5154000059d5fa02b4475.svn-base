using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.Helpers;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Model.Helpers
{
    public class DefaultResultBitOptionSeedingStrategy : IResultBitOptionSeedingStrategy
    {
        private readonly ITypesContainer _container;

        public DefaultResultBitOptionSeedingStrategy(ITypesContainer container)
        {
            this._container = container;
        }

        #region Implementation of IResultBitOptionSeedingStrategy

        public List<IBitOption> UpdateBitOptions(IMatrixTemplate matrixTemplate, List<IBitOption> existingBitOptions)
        {
            List<IBitOption> resultBitOptions = new List<IBitOption>();
            foreach (IVariableSignature variableOptionSignature in matrixTemplate.VariableOptionSignatures)
            {
                if (matrixTemplate.MatrixVariableOptionTemplate is ListMatrixVariableOptionTemplate)
                {
                    foreach (Interfaces.Model.OptionTemplates.IOptionPossibleValue optionPossibleValue in
                        (matrixTemplate.MatrixVariableOptionTemplate as ListMatrixVariableOptionTemplate)
                        .OptionPossibleValues)
                    {
                        IBitOption bitOption = this._container.Resolve<IBitOption>(MatrixKeys.LIST_MATRIX_BIT_OPTION);
                        ((ListMatrixBitOption)bitOption).OptionPossibleValue = optionPossibleValue;
                        bitOption.VariableSignature = variableOptionSignature;

                        IBitOption existing =
                            existingBitOptions.FirstOrDefault((option => option.IsBitOptionEqual(bitOption)));
                        resultBitOptions.Add(existing ?? bitOption);
                    }
                }
                else if (matrixTemplate.MatrixVariableOptionTemplate is BoolMatrixVariableOptionTemplate)
                {
                    IBitOption bitOption = this._container.Resolve<IBitOption>(MatrixKeys.BOOL_MATRIX_BIT_OPTION);
                    bitOption.VariableSignature = variableOptionSignature;

                    IBitOption existing =
                        existingBitOptions.FirstOrDefault((option => option.IsBitOptionEqual(bitOption)));
                    resultBitOptions.Add(existing ?? bitOption);
                }
            }

            return resultBitOptions;
        }

        #endregion
    }
}
