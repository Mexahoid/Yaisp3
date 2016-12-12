﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AgencySimulator.Interfaces
{
    /// <summary>
    /// Интерфейс отрисовываемого объекта.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Метод отрисовки.
        /// </summary>
        /// <param name="Graphics">Канва, на которой производится отрисовка.</param>
        void Draw(Graphics Graphics);

        /// <summary>
        /// Установка координат.
        /// </summary>
        /// <param name="T">Кортеж линейных координат.</param>
        void SetDims(Tuple<int, int, double, double, double, double> T);
    }
}
