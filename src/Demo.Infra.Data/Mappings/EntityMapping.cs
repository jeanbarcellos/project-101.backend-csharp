﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Demo.Core.Domain;
using Demo.Core.Extensions;

namespace Demo.Infra.Data.Mappings
{
    public abstract class EntityMapping<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        private string _tableName;

        protected EntityMapping<T> SetTableName(string name)
        {
            _tableName = name;
            return this;
        }

        protected string GetTableName()
        {
            if (_tableName == null)
            {
                return typeof(T).Name.ToSnakeCase();
            }

            return _tableName;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            ConfigureTable(builder);
            ConfigurePrimaryKey(builder);
            ConfigureIdColumn(builder);
        }

        private EntityTypeBuilder<T> ConfigureTable(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(GetTableName());

            return builder;
        }

        private EntityTypeBuilder<T> ConfigurePrimaryKey(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(c => c.Id)
                .HasName($"{GetTableName()}_pk");

            return builder;
        }

        private EntityTypeBuilder<T> ConfigureIdColumn(EntityTypeBuilder<T> builder)
        {
            builder.Property(c => c.Id)
                .IsRequired()
                .HasColumnName("serial")
                .HasColumnName("id")
                .ValueGeneratedOnAdd(); ;

            return builder;
        }

        protected string GenerateForeignKeyName(string columnName)
        {
            return $"{GetTableName()}_{columnName}_fk";
        }

        protected string GenerateIndexName(string columnName)
        {
            return $"{GetTableName()}_{columnName}_ix";
        }
    }
}
