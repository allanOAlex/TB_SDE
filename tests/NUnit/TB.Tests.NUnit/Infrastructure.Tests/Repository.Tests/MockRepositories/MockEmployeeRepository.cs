﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Shared.Dtos;

namespace TB.Tests.NUnit.Infrastructure.Tests.Repository.Tests.MockRepositories
{
    internal sealed class MockEmployeeRepository
    {
        private readonly IConfiguration configuration;
        private readonly MyDBContext context;

        public MockEmployeeRepository(IConfiguration Configuration, MyDBContext Context)
        {
            configuration = Configuration;
            context = Context;
        }


        public Task<Employee> TestUpdatesEmployeeSalary(Employee employee, out int oldSalary)
        {
            try 
            {
                using (IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("TBMS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@empID", employee.Id);
                    parameters.Add("@newSalary", employee.Salary);
                    parameters.Add("@oldSalary", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = connection.ExecuteAsync("UpdateEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);

                    object oldSalaryObj = parameters.Get<object>("@oldSalary");
                    oldSalary = (oldSalaryObj != DBNull.Value) ? Convert.ToInt32(oldSalaryObj) : 0;

                    if (!string.IsNullOrEmpty(oldSalary.ToString()))
                    {
                        return Task.FromResult(employee);
                    }

                    throw new Exception("Error updating employee salary");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UpdateEmployeeDto> TestUpdatesEmployeeSalary_RollBack(Employee employee)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("TBMS")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@empID", employee.Id);
                    parameters.Add("@newSalary", employee.Salary);
                    parameters.Add("@oldSalary", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("UpdateEmployeeSalary", parameters, commandType: CommandType.StoredProcedure);

                    object updatedOldSalary = parameters.Get<int>("@oldSalary");
                    int oldSalary = (updatedOldSalary != DBNull.Value) ? Convert.ToInt32(updatedOldSalary) : 0;

                    return oldSalary != 0 ? new UpdateEmployeeDto { Succesful = true, Message = "Salary updated successfully!", Id = employee.Id, OldSalary = oldSalary, Salary = employee.Salary } : new UpdateEmployeeDto { Succesful = true, Message = "Failed updating employee salary", Id = employee.Id, OldSalary = oldSalary, Salary = employee.Salary };
                }
            }
            catch (Exception )
            {
                throw;
            }
        }


    }
}
