using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zippy.Models;

namespace Zippy.Data.Contract
{
    public interface IZDbDriver : IDisposable
    {
        Task<Person> FindPersonAsync(string name);
        Task<ISet<Person>> FindPersonsAsync(string zip);
        Task UpsertAsync(Person person);
        Task SaveAsync(Person person);
        Task<bool> UpdateAsync(Person person);
    }
}