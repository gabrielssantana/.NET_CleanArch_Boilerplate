namespace AC.Core.Application.Commands
{
    public class CommandResult
    {
        public bool Success { get; set; } = false;
        public List<string> Messages { get; set; }
        public CommandResult()
        {
            Messages = new();
        }
        public CommandResult(List<string> messages)
        {
            Messages = messages;
        }
        public virtual CommandResult IsSuccess()
        {
            Success = true;
            return this;
        }
        public virtual CommandResult IsSuccess(string mensagem)
        {
            IsSuccess();
            Messages.Add(mensagem);
            return this;
        }
        public virtual CommandResult IsSuccess(IEnumerable<string> mensagens)
        {
            IsSuccess();
            Messages.AddRange(mensagens);
            return this;
        }
        private protected virtual CommandResult IsError()
        {
            Success = false;
            return this;
        }
        public virtual CommandResult AddError(string erro)
        {
            IsError();
            Messages.Add(erro);
            return this;
        }
        public virtual CommandResult AddErrors(IEnumerable<string> erros)
        {
            IsError();
            Messages.AddRange(erros);
            return this;
        }
    }

    public class CommandResult<T> : CommandResult
    {
        public CommandResult() : base() { }
        public CommandResult(List<string> messages) : base(messages) { }
        public T? Data { get; set; }
        public override CommandResult<T> IsSuccess()
        {
            base.IsSuccess();
            return this;
        }
        public CommandResult<T> IsSuccess(T? data)
        {
            IsSuccess();
            Data = data;
            return this;
        }
        public override CommandResult<T> IsSuccess(string mensagem)
        {
            base.IsSuccess(mensagem);
            return this;
        }
        public override CommandResult<T> IsSuccess(IEnumerable<string> mensagens)
        {
            base.IsSuccess(mensagens);
            return this;
        }
        public CommandResult<T> IsSuccess(T? data, string mensagem)
        {
            IsSuccess(data);
            IsSuccess(mensagem);
            return this;
        }
        public CommandResult<T> IsSuccess(T? data, IEnumerable<string> mensagens)
        {
            IsSuccess(data);
            IsSuccess(mensagens);
            return this;
        }
        private protected override CommandResult<T> IsError()
        {
            base.IsError();
            Data = default;
            return this;
        }
        public override CommandResult<T> AddError(string erro)
        {
            IsError();
            base.AddError(erro);
            return this;
        }
        public override CommandResult<T> AddErrors(IEnumerable<string> erros)
        {
            IsError();
            base.AddErrors(erros);
            return this;
        }
    }
}