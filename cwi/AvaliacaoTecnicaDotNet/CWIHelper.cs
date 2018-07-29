using System;

namespace AllanSerraVasconcellos
{
    public static class CWIHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">Data formatada no padão "dd/MM/yyyy HH24:mi" </param>
        /// <param name="op">Operação, '+' ou '-' </param>
        /// <param name="value">Valor em minutos que deve ser acrescentado ou decrementado</param>
        /// <returns></returns>
        public static string ChangeDate(string date, char op, long value)
        {
            var cwidate = new CWIDate(date);

            if (!cwidate.IsValidateDate)
            {
                throw new FormatException("Formato da data está inválido");
            }

            // Se o valor for menor que zero, o sinal deve ser ignorado (tratar como positivo) 
            if (value < 0)
            {
                value *= -1;
            }

            switch (op)
            {
                case '+':
                    cwidate.AddMinuto(value);
                    break;
                case '-':
                    cwidate.DecMinuto(value);
                    break;
                default:
                    throw new ArgumentException("Operador informado não válido");
            }

            return cwidate.CurrentDate;
        }
    }
}
