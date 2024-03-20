namespace Algorithm
{
    
    /// <summary>
    /// 非空的结果，如果`value`为`null`，一样会判定为`fail`
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ValuedResult<T>
    {
        public bool isSuccess => value != null && _success;

        private readonly bool _success;

        public readonly T value;

        public ValuedResult(bool success, T value)
        {
            this.value = value;
            _success = success;
        }

        public static ValuedResult<T> Success(T value)
        {
            return new ValuedResult<T>(true, value);
        }
        
        public static ValuedResult<T> Fail(T value)
        {
            return new ValuedResult<T>(false, value);
        }

    }
}