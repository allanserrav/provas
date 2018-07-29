using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AllanSerraVasconcellos
{
    public class CWIDate
    {
        private Dictionary<string, long> _mapdate;

        public bool IsValidateDate { get; }

        /// <summary>
        /// A data em texto atualmente modificada.
        /// </summary>
        public string CurrentDate {
            get {
                return String.Format("{0:00}/{1:00}/{2:0000} {3:00}:{4:00}",
                    _mapdate[DD], _mapdate[MM], _mapdate[YYYY], _mapdate[HH24], _mapdate[MI]);
            }
        }

        // Constantes para as chaves do mapeamento da data
        private const string MI = "mi";
        private const string HH24 = "HH24";
        private const string YYYY = "yyyy";
        private const string MM = "MM";
        private const string DD = "dd";

        public CWIDate(string date)
        {
            _mapdate = new Dictionary<string, long>();

            // validação da data informada com regex
            var rgx = new Regex(@"^(0[1-9]|[12][0-9]|3[01])/([0][1-9]|[1][0-2])/([1-2][0-9][0-9][0-9]) (?:[01][0-9]|2[0-3]):[0-5][0-9]$");
            IsValidateDate = rgx.IsMatch(date);

            if (IsValidateDate)
            {
                // dd/MM/yyyy HH:mi
                _mapdate.Add(DD, Convert.ToInt64(date.Substring(0, 2))); // Dia
                _mapdate.Add(MM, Convert.ToInt64(date.Substring(3, 2))); // Mês
                _mapdate.Add(YYYY, Convert.ToInt64(date.Substring(6, 4))); // Ano
                _mapdate.Add(HH24, Convert.ToInt64(date.Substring(11, 2))); // horas
                _mapdate.Add(MI, Convert.ToInt64(date.Substring(14, 2))); // minutos

                // validação para verificar se o dia informado está de acordo com o dia limite no mês informado
                long mesdias = GetMesDias(_mapdate[MM]);
                IsValidateDate = _mapdate[DD] > mesdias ? false : IsValidateDate;
            }
        }

        #region Add

        /// <summary>
        /// Adiciona minutos a data corrente.
        /// </summary>
        /// <param name="value">Minutos</param>
        public void AddMinuto(long value)
        {
            long minutoAtual = _mapdate[MI];
            _mapdate[MI] = CalcularAddFracao(value, minutoAtual, 60, () => AddHora(1));
        }

        /// <summary>
        /// Adiciona horas a data corrente.
        /// </summary>
        /// <param name="value">Horas</param>
        public void AddHora(long value)
        {
            long horaAtual = _mapdate[HH24];
            _mapdate[HH24] = CalcularAddFracao(value, horaAtual, 24, () => AddDia(1));
        }

        /// <summary>
        /// Adiciona dias a data corrente.
        /// </summary>
        /// <param name="value">Dias</param>
        public void AddDia(long value)
        {
            long diaAtual = _mapdate[DD];
            long mesAtual = _mapdate[MM];
            decimal fracao = GetMesDias(mesAtual); // fração é a quantidade de dias que tem o mês
            _mapdate[DD] = CalcularAddFracao(value, diaAtual, fracao, () => AddMes(1));
        }

        /// <summary>
        /// Adiciona meses a data corrente.
        /// </summary>
        /// <param name="value">Meses</param>
        public void AddMes(long value)
        {
            long mesAtual = _mapdate[MM];
            _mapdate[MM] = CalcularAddFracao(value, mesAtual, 12, () => AddAno(1));
        }

        /// <summary>
        /// Adiciona anos a data corrente.
        /// </summary>
        /// <param name="value">Anos</param>
        public void AddAno(long value)
        {
            _mapdate[YYYY] += value;
        }

        private long CalcularAddFracao(long value, long atual, decimal fracao, Action fracaoAction)
        {
            long atualizado = value + atual;

            decimal q = atualizado / fracao;
            for (; q >= 1; q--)
            {
                fracaoAction();
            }

            return Convert.ToInt64(q * fracao);
        }

        #endregion

        #region Dec

        /// <summary>
        /// Decrementa minutos da data corrente.
        /// </summary>
        /// <param name="value">Minutos</param>
        public void DecMinuto(long value)
        {
            long minutoAtual = _mapdate[MI];
            _mapdate[MI] = CalcularDecFracao(value, minutoAtual, 60, () => DecHora(1));
        }

        /// <summary>
        /// Decrementa horas da data corrente.
        /// </summary>
        /// <param name="value">Horas</param>
        public void DecHora(long value)
        {
            long horaAtual = _mapdate[HH24];
            _mapdate[HH24] = CalcularDecFracao(value, horaAtual, 24, () => DecDia(1));
        }

        /// <summary>
        /// Decrementa dias da data corrente.
        /// </summary>
        /// <param name="value">Dias</param>
        public void DecDia(long value)
        {
            long diaAtual = _mapdate[DD];
            long mesAnterior = _mapdate[MM] - 1;
            mesAnterior = mesAnterior <= 0 ? 12 : mesAnterior;
            decimal fracao = GetMesDias(mesAnterior); // fração é a quantidade de dias que tem o mês
            _mapdate[DD] = CalcularDecFracao(value, diaAtual, fracao, () => DecMes(1), 1);
        }

        /// <summary>
        /// Decrementa meses da data corrente.
        /// </summary>
        /// <param name="value">Meses</param>
        public void DecMes(long value)
        {
            long mesAtual = _mapdate[MM];
            _mapdate[MM] = CalcularDecFracao(value, mesAtual, 12, () => DecAno(1), 1);
        }

        /// <summary>
        /// Decrementa anos da data corrente.
        /// </summary>
        /// <param name="value">Anos</param>
        public void DecAno(long value)
        {
            _mapdate[YYYY] -= value;
        }

        private long CalcularDecFracao(long value, long atual, decimal fracao, Action fracaoAction, long primeiro = 0)
        {
            long atualizada = atual - value;

            if (atualizada >= primeiro)
            {
                return atualizada;
            }

            atualizada *= -1;
            decimal q = (atualizada / fracao) + 1;
            for (; q >= 1; q--)
            {
                fracaoAction();
            }

            return Convert.ToInt64(fracao - (q * fracao));
        }

        #endregion

        /// <summary>
        /// Obter a quantidade de dias que tem no mês
        /// </summary>
        /// <param name="mes">Mês de verificação</param>
        private long GetMesDias(long mes)
        {
            if (mes == 2) // Ignore o fato de fevereiro poder possuir 28 ou 29 dias. Considere-o sempre com 28 
                return 28;
            else if (mes % 2 == 0) // mês par
                return 30;
            else // mês impar
                return 31;
        }
    }
}
