using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Common;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure.Data;
using System.Text.RegularExpressions;
using System.Text;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductEntity>> ListAsync(Filter filter)
        {
            if (filter == null) return await _context.Products.ToListAsync();

            IQueryable<ProductEntity> query = _context.Products;
            if (filter.Id.HasValue)            
                query = query.Where(p => p.Id == filter.Id);

            else if (filter.Name != null)
                query = query.Where(p => p.Name.Contains(filter.Name));

            else if (!string.IsNullOrEmpty(filter.Order))
                query = filter.Ascending
                    ? query.OrderBy(p => EF.Property<object>(p, ToPascalCase(filter.Order.ToLower())))
                    : query.OrderByDescending(p => EF.Property<object>(p, ToPascalCase(filter.Order.ToLower())));

            return await query.ToListAsync();
        }

        private static string ToPascalCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var words = Regex.Split(name, @"[^\p{L}\p{Nd}]");

            var pascalWord = words.Aggregate(new StringBuilder(), (sb, word) =>
            {
                if (sb.Length > 0)
                {
                    sb.Append(char.ToLowerInvariant(word[0]));
                    sb.Append(word.Substring(1));
                }
                else                
                    sb.Append(word);
                return sb;
            }).ToString();

            return char.ToUpperInvariant(pascalWord [0]) + pascalWord .Substring(1);
        }

        public async Task<bool> AddAsync(ProductEntity product)
        {
            if (ProductDetailsExists(product.Id))
                return false;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ProductEntity product)
        {
            var isProduct = ProductDetailsExists(product.Id);
            if (isProduct)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private bool ProductDetailsExists(int productId) => _context.Products.Any(e => e.Id == productId);

        public async Task<bool> DeleteAsync(int productId)
        {
            var findProductData = _context.Products.Where(_ => _.Id == productId).FirstOrDefault();
            if (findProductData != null)
            {
                _context.Products.Remove(findProductData);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
