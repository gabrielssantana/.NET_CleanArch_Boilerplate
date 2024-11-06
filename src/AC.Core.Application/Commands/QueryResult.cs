using AC.Core.Application.Interfaces;
using AC.Core.Domain.Interfaces.Queries;

using AutoMapper;

namespace AC.Core.Application.Commands
{
    public class QueryResult<T> : CommandResult, IMap
    {
        public QueryResult() : base() { }
        public QueryResult(List<string> messages) : base(messages) { }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public override QueryResult<T> IsSuccess()
        {
            base.IsSuccess();
            return this;
        }
        public QueryResult<T> IsSuccess(IEnumerable<T> data)
        {
            IsSuccess();
            Data = data;
            return this;
        }
        public override QueryResult<T> IsSuccess(string mensagem)
        {
            base.IsSuccess(mensagem);
            return this;
        }
        public override QueryResult<T> IsSuccess(IEnumerable<string> mensagens)
        {
            base.IsSuccess(mensagens);
            return this;
        }
        public QueryResult<T> IsSuccess(IEnumerable<T> data, string mensagem)
        {
            IsSuccess(data);
            IsSuccess(mensagem);
            return this;
        }
        public QueryResult<T> IsSuccess(IEnumerable<T> data, IEnumerable<string> mensagens)
        {
            IsSuccess(data);
            IsSuccess(mensagens);
            return this;
        }
        private protected override QueryResult<T> IsError()
        {
            base.IsError();
            Data = Enumerable.Empty<T>();
            return this;
        }
        public override QueryResult<T> AddError(string erro)
        {
            IsError();
            base.AddError(erro);
            return this;
        }
        public override QueryResult<T> AddErrors(IEnumerable<string> erros)
        {
            IsError();
            base.AddErrors(erros);
            return this;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(AC.Core.Domain.DTO.Queries.QueryResult<>), typeof(QueryResult<>));
        }
    }
}