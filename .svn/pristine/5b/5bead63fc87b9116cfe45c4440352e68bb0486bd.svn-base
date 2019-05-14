using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Oscilloscope
{
    /// <summary>
    /// Класс со вспомогательными методами
    /// </summary>
   public static class CommonHelper
    {
       /// <summary>
       /// Создание привязки для управления видимостью контрола
       /// </summary>
       /// <param name="source">Объект источника</param>
       /// <param name="path">Имя свойства</param>
       /// <param name="destination">Контрол</param>
       public static void CreateVisibilityBinding(object source, string path, FrameworkElement destination)
       {
           CreateVisibilityBinding(source, new PropertyPath(path), destination);
       }


       /// <summary>
       /// Создание привязки для управления видимостью контрола
       /// </summary>
       /// <param name="source">Объект источника</param>
       /// <param name="path">Имя свойства</param>
       /// <param name="destination">Контрол</param>
       public static void CreateVisibilityBinding(object source, PropertyPath path, FrameworkElement destination)
       {
           Binding binding = new Binding();
           binding.Converter = new BooleanToVisibilityConverter();
           binding.Source = source;
           binding.Path = path;
           destination.SetBinding(UIElement.VisibilityProperty, binding);
       }

       /// <summary>
       /// Создание привязки для управления состоянием CheckBox
       /// </summary>
       /// <param name="source">Объект источника</param>
       /// <param name="path">Имя свойства</param>
       /// <param name="destination">CheckBox</param>
          public static void CreateCheckedBinding(object source, string path, FrameworkElement destination)
          {
              var binding = new Binding();
              binding.Source = source;
              binding.Path = new PropertyPath(path);
              destination.SetBinding(MenuItem.IsCheckedProperty, binding);
          }
       /// <summary>
       /// Рассчёт шага сетки по оси ОХ
       /// </summary>
       /// <param name="max">Максимальное расстояние между отсчётами (px)</param>
       /// <param name="width">Ширина видимой области(px)</param>
       /// <param name="lenght">Кол-во значений по Х</param>
       /// <returns></returns>
          public static int CalcBaseXValue(int max, double width, double lenght)
          {
              double koeff = lenght / width;
              max = (int)(max * koeff);

              int digitCount = (int)Math.Log10(max) + 1;

              int res;

              max = (int)(max / Math.Pow(10, digitCount - 2));

              if (max / 50.0 >= 1)
              {
                  res = 50;
              }
              else
              {
                  if (max / 25.0 >= 1)
                  {
                      res = 25;
                  }
                  else
                  {
                      if (max / 20.0 >= 1)
                      {
                          res = 20;
                      }
                      else
                      {
                          res = 10;
                      }
                  }
              }

              return (int)(res * Math.Pow(10, digitCount - 2));

          }
    }
}
