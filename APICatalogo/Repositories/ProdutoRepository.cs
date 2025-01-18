﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //         .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
    //        .Take(produtosParams.PageSize)
    //        .ToList();
    // }
    // }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos.OrderBy(p => p.Nome).AsQueryable();

        var resultado = await produtosOrdenados.ToPagedListAsync(
            produtosParams.PageNumber,
            produtosParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
        {
            if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }
        var produtosFiltrados = await produtos.ToPagedListAsync(
            produtosFiltroParams.PageNumber,
            produtosFiltroParams.PageSize);
    
        return produtosFiltrados;
    }
    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();
        var produtosCategoria = produtos.Where(p => p.CategoriaId == id);
        return produtosCategoria;
    }
}
