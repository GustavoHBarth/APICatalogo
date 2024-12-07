using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // ele vai estar atendendo em mais de uma rota
        // usando /primeiro vai ignorar a rota definida anteriormente.
        [HttpGet("/primeiro")]
        [HttpGet("primeiro")]
        public ActionResult <Produto> GetPrimeiro()
        {
            var produto =_context.Produtos.FirstOrDefault();
            if(produto is null)
            {
                return NotFound();
            }
            return produto;
        }

        // usando metodo async 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }



        [HttpGet("{id}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get (int id, [BindRequired] string name)
        {

           var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null) 
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null) 
                return BadRequest();
           

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", 
                new {id = produto.ProdutoId}, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) 
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok (produto);
        }

         [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não localizado");
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
