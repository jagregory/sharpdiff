using OMetaSharp;

namespace SharpDiff
{
    public class DiffParser : Parser
    {
        public virtual bool FormatType(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> format = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("diff --").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Text, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    format = result2;
                    result2 = ( new FormatType(format.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Text(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            modifiedStream = inputStream;
            if(!MetaRules.Many1(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Apply(LetterOrDigit, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    return MetaRules.Success();
                }
            , modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
    }
}
