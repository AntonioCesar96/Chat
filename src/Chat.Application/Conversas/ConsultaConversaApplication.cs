﻿using Chat.Application.Conversas.Interfaces;
using Chat.Domain.Common;
using Chat.Domain.Conversas.Dtos;
using Chat.Domain.Conversas.Interfaces;

namespace Chat.Application.Conversas
{
    public class ConsultaConversaApplication : IConsultaConversaApplication
    {
        private readonly IConsultaConversa _consultaConversas;

        public ConsultaConversaApplication(IConsultaConversa consultaConversas)
        {
            _consultaConversas = consultaConversas;
        }

        public ResultadoDaConsulta ObterConversasDoContato(ConversaFiltroDto filtro)
        {
            var resultado = _consultaConversas.ObterConversasDoContato(filtro);
            return resultado;
        }
    }
}
