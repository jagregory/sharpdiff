using System.Collections.Generic;
using SharpDiff.FileStructure;
using OMetaSharp;

namespace SharpDiff
{
    public class GitDiffParser : Parser
    {
        public virtual bool Diffs(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> diffs = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(
                                delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                                {
                                    modifiedStream4 = inputStream4;
                                    if(!MetaRules.Or(modifiedStream4, out result4, out modifiedStream4,
                                    delegate(OMetaStream<char> inputStream5, out OMetaList<HostExpression> result5, out OMetaStream <char> modifiedStream5)
                                    {
                                        modifiedStream5 = inputStream5;
                                        if(!MetaRules.Apply(NewLine, modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        return MetaRules.Success();
                                    }
                                    ,delegate(OMetaStream<char> inputStream5, out OMetaList<HostExpression> result5, out OMetaStream <char> modifiedStream5)
                                    {
                                        modifiedStream5 = inputStream5;
                                        if(!MetaRules.Apply(Empty, modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        return MetaRules.Success();
                                    }
                                    ))
                                    {
                                        return MetaRules.Fail(out result4, out modifiedStream4);
                                    }
                                    if(!MetaRules.Apply(Diff, modifiedStream4, out result4, out modifiedStream4))
                                    {
                                        return MetaRules.Fail(out result4, out modifiedStream4);
                                    }
                                    return MetaRules.Success();
                                }, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    diffs = result2;
                    result2 = ( diffs ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Diff(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> header = null;
            OMetaList<HostExpression> chunks = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Apply(Header, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    header = result2;
                    if(!MetaRules.Apply(NewLine, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(IndexHeader, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(NewLine, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Chunks, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    chunks = result2;
                    result2 = ( new Diff(header.As<Header>(), chunks.ToIEnumerable<Chunk>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Header(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> format = null;
            OMetaList<HostExpression> files = null;
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("diff").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Space, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(FormatType, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        format = result3;
                        if(!MetaRules.Apply(FileDefs, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        files = result3;
                        result3 = ( new Header(format.As<FormatType>(), files.ToIEnumerable<FileDef>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("diff").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Space, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(FormatType, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        format = result3;
                        result3 = ( new Header(format.As<FormatType>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool IndexHeader(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> range = null;
            OMetaList<HostExpression> mode = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("index").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(HashRange, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    range = result2;
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Number, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    mode = result2;
                    result2 = ( new IndexHeader(range.As<HashRange>(), mode.As<int>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Chunks(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> chunks = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Or(modifiedStream2, out result2, out modifiedStream2,
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(
                            delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                            {
                                modifiedStream4 = inputStream4;
                                if(!MetaRules.Apply(ChunkHeader, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                if(!MetaRules.Apply(NewLine, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                return MetaRules.Success();
                            }, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        return MetaRules.Success();
                    }
                    ,delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(Empty, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        return MetaRules.Success();
                    }
                    ))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(Chunk, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    chunks = result2;
                    result2 = ( chunks ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Chunk(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> range = null;
            OMetaList<HostExpression> lines = null;
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(ChunkRange, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        range = result3;
                        if(!MetaRules.Or(modifiedStream3, out result3, out modifiedStream3,
                        delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(NewLine, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ,delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(Empty, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(DiffLines, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        lines = result3;
                        if(!MetaRules.Or(modifiedStream3, out result3, out modifiedStream3,
                        delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(NewLine, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ,delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(Empty, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        result3 = ( new Chunk(range.As<ChunkRange>(), lines.ToIEnumerable<ILine>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(ChunkRange, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        range = result3;
                        if(!MetaRules.Or(modifiedStream3, out result3, out modifiedStream3,
                        delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(NewLine, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ,delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                        {
                            modifiedStream4 = inputStream4;
                            if(!MetaRules.Apply(Empty, modifiedStream4, out result4, out modifiedStream4))
                            {
                                return MetaRules.Fail(out result4, out modifiedStream4);
                            }
                            return MetaRules.Success();
                        }
                        ))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        result3 = ( new Chunk(range.As<ChunkRange>(), new List<ILine>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool ChunkHeader(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> originalFile = null;
            OMetaList<HostExpression> newFile = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("---").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(FileDef, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    originalFile = result2;
                    if(!MetaRules.Apply(NewLine, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("+++").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(FileDef, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    newFile = result2;
                    result2 = ( new ChunkHeader(originalFile.As<FileDef>(), newFile.As<FileDef>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool ChunkRange(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> originalRange = null;
            OMetaList<HostExpression> newRange = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("@@").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(ChangeRange, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    originalRange = result2;
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(ChangeRange, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    newRange = result2;
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("@@").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    result2 = ( new ChunkRange(originalRange.As<ChangeRange>(), newRange.As<ChangeRange>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool ChangeRange(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> line = null;
            OMetaList<HostExpression> affected = null;
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("+").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Number, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        line = result3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, (",").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Number, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        affected = result3;
                        result3 = ( new ChangeRange(line.As<int>(), affected.As<int>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("-").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Number, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        line = result3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, (",").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Number, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        affected = result3;
                        result3 = ( new ChangeRange(line.As<int>(), affected.As<int>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool DiffLines(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> lines = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(AnyLine, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    lines = result2;
                    result2 = ( lines ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool AnyLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(ContextLine, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(AdditionLine, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(SubtractionLine, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(NoNewLineAtEOFLine, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool ContextLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> value = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Apply(Space, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Line, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    value = result2;
                    result2 = ( new ContextLine(value.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool AdditionLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> value = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("+").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Line, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    value = result2;
                    result2 = ( new AdditionLine(value.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool SubtractionLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> value = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("-").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Apply(Line, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    value = result2;
                    result2 = ( new SubtractionLine(value.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool NoNewLineAtEOFLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Exactly, modifiedStream3, out result3, out modifiedStream3, ("\\").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Space, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("No newline at end of file").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(NewLine, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        result3 = ( new NoNewLineAtEOFLine() ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Exactly, modifiedStream3, out result3, out modifiedStream3, ("\\").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Space, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("No newline at end of file").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        result3 = ( new NoNewLineAtEOFLine() ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Line(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> value = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Or(modifiedStream2, out result2, out modifiedStream2,
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(Text, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        return MetaRules.Success();
                    }
                    ,delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(
                            delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                            {
                                modifiedStream4 = inputStream4;
                                if(!MetaRules.Apply(Empty, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                result4 = ( "" ).AsHostExpressionList();
                                return MetaRules.Success();
                            }, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        return MetaRules.Success();
                    }
                    ))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    value = result2;
                    if(!MetaRules.Apply(NewLine, modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    result2 = ( value.As<string>() ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool FileDefs(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> files = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(FileDef, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    files = result2;
                    result2 = ( files ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool FileDef(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> letter = null;
            OMetaList<HostExpression> filename = null;
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(Space, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Letter, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        letter = result3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("/").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Filename, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        filename = result3;
                        result3 = ( new FileDef(letter.As<char>(), filename.As<string>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Apply(Letter, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        letter = result3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, ("/").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Apply(Filename, modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        filename = result3;
                        result3 = ( new FileDef(letter.As<char>(), filename.As<string>()) ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool FormatType(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> format = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("--").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(LetterOrDigit, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
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
        public virtual bool HashRange(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> first = null;
            OMetaList<HostExpression> second = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(LetterOrDigit, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    first = result2;
                    if(!MetaRules.ApplyWithArgs(Token, modifiedStream2, out result2, out modifiedStream2, ("..").AsHostExpressionList()))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(LetterOrDigit, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    second = result2;
                    result2 = ( new HashRange(first.As<string>(), second.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Filename(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> filename = null;
            OMetaList<HostExpression> extension = null;
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Many1(
                            delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                            {
                                modifiedStream4 = inputStream4;
                                if(!MetaRules.Apply(
                                    delegate(OMetaStream<char> inputStream5, out OMetaList<HostExpression> result5, out OMetaStream <char> modifiedStream5)
                                    {
                                        modifiedStream5 = inputStream5;
                                        if(!MetaRules.Not(
                                            delegate(OMetaStream<char> inputStream6, out OMetaList<HostExpression> result6, out OMetaStream <char> modifiedStream6)
                                            {
                                                modifiedStream6 = inputStream6;
                                                if(!MetaRules.Apply(Space, modifiedStream6, out result6, out modifiedStream6))
                                                {
                                                    return MetaRules.Fail(out result6, out modifiedStream6);
                                                }
                                                return MetaRules.Success();
                                            }
                                        , modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        if(!MetaRules.Not(
                                            delegate(OMetaStream<char> inputStream6, out OMetaList<HostExpression> result6, out OMetaStream <char> modifiedStream6)
                                            {
                                                modifiedStream6 = inputStream6;
                                                if(!MetaRules.Apply(NewLine, modifiedStream6, out result6, out modifiedStream6))
                                                {
                                                    return MetaRules.Fail(out result6, out modifiedStream6);
                                                }
                                                return MetaRules.Success();
                                            }
                                        , modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        if(!MetaRules.Apply(Character, modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        return MetaRules.Success();
                                    }, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                return MetaRules.Success();
                            }
                        , modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        filename = result3;
                        if(!MetaRules.ApplyWithArgs(Token, modifiedStream3, out result3, out modifiedStream3, (".").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.Many1(
                            delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                            {
                                modifiedStream4 = inputStream4;
                                if(!MetaRules.Apply(LetterOrDigit, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                return MetaRules.Success();
                            }
                        , modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        extension = result3;
                        result3 = ( filename + "." + extension ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.Many1(
                            delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                            {
                                modifiedStream4 = inputStream4;
                                if(!MetaRules.Apply(
                                    delegate(OMetaStream<char> inputStream5, out OMetaList<HostExpression> result5, out OMetaStream <char> modifiedStream5)
                                    {
                                        modifiedStream5 = inputStream5;
                                        if(!MetaRules.Not(
                                            delegate(OMetaStream<char> inputStream6, out OMetaList<HostExpression> result6, out OMetaStream <char> modifiedStream6)
                                            {
                                                modifiedStream6 = inputStream6;
                                                if(!MetaRules.Apply(Space, modifiedStream6, out result6, out modifiedStream6))
                                                {
                                                    return MetaRules.Fail(out result6, out modifiedStream6);
                                                }
                                                return MetaRules.Success();
                                            }
                                        , modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        if(!MetaRules.Not(
                                            delegate(OMetaStream<char> inputStream6, out OMetaList<HostExpression> result6, out OMetaStream <char> modifiedStream6)
                                            {
                                                modifiedStream6 = inputStream6;
                                                if(!MetaRules.Apply(NewLine, modifiedStream6, out result6, out modifiedStream6))
                                                {
                                                    return MetaRules.Fail(out result6, out modifiedStream6);
                                                }
                                                return MetaRules.Success();
                                            }
                                        , modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        if(!MetaRules.Apply(Character, modifiedStream5, out result5, out modifiedStream5))
                                        {
                                            return MetaRules.Fail(out result5, out modifiedStream5);
                                        }
                                        return MetaRules.Success();
                                    }, modifiedStream4, out result4, out modifiedStream4))
                                {
                                    return MetaRules.Fail(out result4, out modifiedStream4);
                                }
                                return MetaRules.Success();
                            }
                        , modifiedStream3, out result3, out modifiedStream3))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        filename = result3;
                        result3 = ( filename ).AsHostExpressionList();
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool Text(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            OMetaList<HostExpression> t = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(
                                delegate(OMetaStream<char> inputStream4, out OMetaList<HostExpression> result4, out OMetaStream <char> modifiedStream4)
                                {
                                    modifiedStream4 = inputStream4;
                                    if(!MetaRules.Not(
                                        delegate(OMetaStream<char> inputStream5, out OMetaList<HostExpression> result5, out OMetaStream <char> modifiedStream5)
                                        {
                                            modifiedStream5 = inputStream5;
                                            if(!MetaRules.Apply(NewLine, modifiedStream5, out result5, out modifiedStream5))
                                            {
                                                return MetaRules.Fail(out result5, out modifiedStream5);
                                            }
                                            return MetaRules.Success();
                                        }
                                    , modifiedStream4, out result4, out modifiedStream4))
                                    {
                                        return MetaRules.Fail(out result4, out modifiedStream4);
                                    }
                                    if(!MetaRules.Apply(Character, modifiedStream4, out result4, out modifiedStream4))
                                    {
                                        return MetaRules.Fail(out result4, out modifiedStream4);
                                    }
                                    return MetaRules.Success();
                                }, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    t = result2;
                    result2 = ( t.As<string>() ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public override bool Number(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            Rule<char> __baseRule__ = base.Number;
            OMetaList<HostExpression> ds = null;
            modifiedStream = inputStream;
            if(!MetaRules.Apply(
                delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
                {
                    modifiedStream2 = inputStream2;
                    if(!MetaRules.Many1(
                        delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                        {
                            modifiedStream3 = inputStream3;
                            if(!MetaRules.Apply(Digit, modifiedStream3, out result3, out modifiedStream3))
                            {
                                return MetaRules.Fail(out result3, out modifiedStream3);
                            }
                            return MetaRules.Success();
                        }
                    , modifiedStream2, out result2, out modifiedStream2))
                    {
                        return MetaRules.Fail(out result2, out modifiedStream2);
                    }
                    ds = result2;
                    result2 = ( int.Parse(ds.As<string>()) ).AsHostExpressionList();
                    return MetaRules.Success();
                }, modifiedStream, out result, out modifiedStream))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
        public virtual bool NewLine(OMetaStream<char> inputStream, out OMetaList<HostExpression> result, out OMetaStream <char> modifiedStream)
        {
            modifiedStream = inputStream;
            if(!MetaRules.Or(modifiedStream, out result, out modifiedStream,
            delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.Apply(
                    delegate(OMetaStream<char> inputStream3, out OMetaList<HostExpression> result3, out OMetaStream <char> modifiedStream3)
                    {
                        modifiedStream3 = inputStream3;
                        if(!MetaRules.ApplyWithArgs(Exactly, modifiedStream3, out result3, out modifiedStream3, ("\r").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        if(!MetaRules.ApplyWithArgs(Exactly, modifiedStream3, out result3, out modifiedStream3, ("\n").AsHostExpressionList()))
                        {
                            return MetaRules.Fail(out result3, out modifiedStream3);
                        }
                        return MetaRules.Success();
                    }, modifiedStream2, out result2, out modifiedStream2))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ,delegate(OMetaStream<char> inputStream2, out OMetaList<HostExpression> result2, out OMetaStream <char> modifiedStream2)
            {
                modifiedStream2 = inputStream2;
                if(!MetaRules.ApplyWithArgs(Exactly, modifiedStream2, out result2, out modifiedStream2, ("\n").AsHostExpressionList()))
                {
                    return MetaRules.Fail(out result2, out modifiedStream2);
                }
                return MetaRules.Success();
            }
            ))
            {
                return MetaRules.Fail(out result, out modifiedStream);
            }
            return MetaRules.Success();
        }
    }
}
