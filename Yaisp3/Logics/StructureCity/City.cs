﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yaisp3
{
    public class City
    {
        /// <summary>
        /// Название города
        /// </summary>
        private string cityName;
        /// <summary>
        /// Главная матрица элементов
        /// </summary>
        private Matrices.MatrixElements cityMatrix;

        /// <summary>
        /// Главный конструктор города
        /// </summary>
        /// <param name="Name">Название города</param>
        /// <param name="Width">Ширина города</param>
        /// <param name="Height">Высота города</param>
        public City(string Name, int Width, int Height)
        {
            cityMatrix = new Matrices.MatrixElements(Height, Width);
            cityName = Name;
        }

        /// <summary>
        /// Возвращает название города
        /// </summary>
        /// <returns>Возвращает строку с названием города</returns>
        public string GetName()
        {
            return cityName;
        }

        /// <summary>
        /// Устанавливает новый дом
        /// </summary>
        /// <param name="Row">Строка верхнего левого квадрата дома</param>
        /// <param name="Col">Столбец верхнего левого квадрата дома</param>
        /// <param name="RightWidth">Ширина дома</param>
        /// <param name="DownDepth">Высота дома</param>
        public void PlaceHouse(int Row, int Col, int RightWidth, int DownDepth)
        {
            cityMatrix.PlaceHouse(Row, Col, RightWidth, DownDepth);
        }

        /// <summary>
        /// Достраивает все строящиеся биллборды
        /// </summary>
        public void BuildBillboardsToEnd()
        {
            cityMatrix.BuildAllBillboardsToEnd();
        }

        /// <summary>
        /// Устанавливает биллборд на случайной локации
        /// </summary>
        /// <param name="Billboard"></param>
        public void PlaceBillboard(Billboard Billboard)
        {
            cityMatrix.PlaceBillboard(Billboard);
        }

        /// <summary>
        /// Заполняет случайный биллборд заказом клиента
        /// </summary>
        /// <param name="ClientDesire">Кортеж желания клиента</param>
        public void FillBillboard(Tuple<string, int, byte> ClientDesire)
        {
            cityMatrix.FillRandomBillboard(ClientDesire);
        }

        /// <summary>
        /// Возвращает массив с элементами, заполненными определённым цветом
        /// </summary>
        /// <returns>Возвращает массив цветов</returns>
        public System.Drawing.Color[,] GetDrawingData()
        {
            return cityMatrix.GetDrawingData();
        }

        public System.Drawing.Color[,] GetProximityMap()
        {
            return cityMatrix.GetCoeffMapColors();
        }
    }
}
