using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eStore.Application.DTOs
{
    public class Result<T>
    {
        public Result(bool succeeded, IEnumerable<string> errors, T data)
        {
            Succeeded = succeeded;
            Messages = errors.ToArray();
            Data = data;
        }

        public bool Succeeded { get; set; }
        public string[] Messages { get; set; }
        public T Data { get; set; }

        public static Result<T> Success(string message, T data)
        {
            return new Result<T>(true, new string[] { message }, data);
        }
        public static Result<T> Success(T data)
        {
            return new Result<T>(true, new string[] { "Success" }, data);
        }
        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors,default);
        }
    }
}
