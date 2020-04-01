using System;


namespace Networking.protocols
{
    [Serializable()]
    public class Request 
    {
    private RequestType type { get; set; }
    private Object data { get; set; }

    private Request() { }

    public RequestType Type()
    {
        return type;
    }

    public Object Data()
    {
        return data;
    }

        public override string ToString()
        {
            //return base.ToString();
            return "Request{" + "type='" + type.ToString() + "\'" + ", data='" + data.ToString() + "\'" +"}";
        }



    public class Builder
    {
        private Request request = new Request();

        public Builder Type(RequestType type)
        {
            request.Type(type);
            return this;
        }

        public Builder Data(Object data)
        {
            request.Data(data);
            return this;
        }

        public Request Build()
        {
            return request;
        }
    }

    private void Data(Object data)
    {
        this.data = data;
    }

    private void Type(RequestType type)
    {
        this.type = type;
    }

}

}
