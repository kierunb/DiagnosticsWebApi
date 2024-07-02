﻿using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TargetApp.Models;

namespace TargetApp
{
    public class CategoryRepository
    {
        private IDbConnection DbConnection => new SqlConnection(_dbOptions.ConnectionString);

        private readonly ILogger<CategoryRepository> _logger;
        private readonly ProductDatabaseSettings _dbOptions;

        public CategoryRepository(IOptions<ProductDatabaseSettings> dbOptions, ILogger<CategoryRepository> logger)
        {
            _dbOptions = dbOptions?.Value ?? throw new ArgumentNullException(nameof(dbOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            using var db = DbConnection;

            // Because the DB query is quick, add a Task.Delay to simulate other slower tasks
            Task.Delay(10).Wait();

            return db.Query<ProductCategory>(@"SELECT ProductID, ProductName, SupplierID FROM Products");
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            using var db = DbConnection;

            // Because the DB query is quick, add a Task.Delay to simulate other slower tasks
            await Task.Delay(10);

            return await db.QueryAsync<ProductCategory>("SELECT ProductID, ProductName, SupplierID FROM Products");
        }

        public ProductCategory GetCategory(int id)
        {
            using var db = DbConnection;
            return db.QueryFirstOrDefault<ProductCategory>("SELECT ProductID, ProductName, SupplierID FROM Products WHERE ProductID=@id", new { id });
        }

        public async Task<ProductCategory> GetCategoryAsync(int id)
        {
            using var db = DbConnection;
            return await db.QueryFirstOrDefaultAsync<ProductCategory>("SELECT ProductID, ProductName, SupplierID FROM Products WHERE ProductID=@id", new { id });
        }
    }
}
