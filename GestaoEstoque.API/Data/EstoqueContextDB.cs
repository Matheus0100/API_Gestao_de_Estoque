using GestaoEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque.API.Data
{
    public class EstoqueContextDB : DbContext
    {
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public EstoqueContextDB(DbContextOptions<EstoqueContextDB> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .HasKey(p => p.ID);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("smallmoney");

            modelBuilder.Entity<Produto>()
                .Property(p => p.Descricao)
                .HasColumnType("varchar(MAX)");

            modelBuilder.Entity<Produto>()
                .Property(p => p.QuantidadeEmEstoque)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Produto>()
                .Property(p => p.IdFornecedor)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Produto>().ToTable("Produtos");


            modelBuilder.Entity<Fornecedor>()
                .HasKey(f => f.ID);

            modelBuilder.Entity<Fornecedor>()
                .Property(f => f.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            modelBuilder.Entity<Fornecedor>()
                .Property(f => f.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            modelBuilder.Entity<Fornecedor>()
                .Property(f => f.Ativo)
                .IsRequired()
                .HasColumnType("bit");
            

            modelBuilder.Entity<Fornecedor>().ToTable("Fornecedores");

            base.OnModelCreating(modelBuilder);

            //no console do gerenciador de pacotes utilizar os comandos abaixo:
            //add-migration Initial -context EstoqueContextDB
            //update-database
        }
    }
}
