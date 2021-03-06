﻿using AutoMapper;
using Chat.Domain.Common;
using Chat.Domain.ListaContatos.Dtos;
using Chat.Domain.ListaContatos.Entidades;
using Chat.Domain.ListaContatos.Interfaces;
using Chat.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Infra.Data.Consultas
{
    public class ConsultaListaContato : IConsultaListaContato
    {
        private readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConsultaListaContato(ChatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultadoDaConsulta ObterContatosAmigos(ListaContatoFiltroDto filtro)
        {
            var retorno = new ResultadoDaConsulta();

            var pagina = filtro.Pagina > 0 ? filtro.Pagina : 1;
            var calculoPaginacao = (pagina - 1) * filtro.TotalPorPagina;

            IQueryable<ListaContato> listaContatos = CriarConsultaDeListaContatos(filtro);

            retorno.Pagina = pagina;
            retorno.TotalPorPagina = filtro.TotalPorPagina;
            retorno.Total = listaContatos.Count();
            retorno.Lista = _mapper.Map<List<ListaAmigosDto>>(listaContatos
                    .Skip(calculoPaginacao)
                    .Take(filtro.TotalPorPagina));

            return retorno;
        }

        private IQueryable<ListaContato> CriarConsultaDeListaContatos(ListaContatoFiltroDto filtro)
        {
            return _dbContext.Set<ListaContato>()
                .Include(x => x.ContatoAmigo)
                .Where(p =>
                    (!filtro.ContatosIdsParaIgnorar.Any() || !filtro.ContatosIdsParaIgnorar.Any(id => id == p.ContatoAmigoId))
                    && p.ContatoPrincipalId == filtro.ContatoPrincipalId
                    && (string.IsNullOrEmpty(filtro.NomeAmigo) || p.ContatoAmigo.Nome.Contains(filtro.NomeAmigo.Trim().ToLower()))
                    && (string.IsNullOrEmpty(filtro.EmailAmigo) || p.ContatoAmigo.Email.Contains(filtro.EmailAmigo.Trim().ToLower()))
                );
        }
    }
}
