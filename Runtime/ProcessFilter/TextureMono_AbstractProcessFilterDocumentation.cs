using System;
using UnityEngine;

public abstract class TextureMono_AbstractProcessFilterDocumentation : MonoBehaviour, I_OwnProcessFilterDocumentation
{
    public abstract STRUCT_TextureProcessFilterTextInfo GetProcessFilterTextInfo();

    internal void GetCallTextId(out string id)
    {
       id=GetProcessFilterTextInfo().m_callTextId;
    }

    internal void GetCreditName(out string creditName)
    {
        creditName = GetProcessFilterTextInfo().m_creatorName;
    }

    internal void GetCreditUrl(out string creditUrl)
    {
        creditUrl = GetProcessFilterTextInfo().m_creatorContactUrl;
    }

    internal void GetProcessDescription(out string description)
    {
        description = GetProcessFilterTextInfo().m_processDescription;
    }

    internal void GetProcessLearnMoreUrl(out string url)
    {
        url = GetProcessFilterTextInfo().m_urlToLearnMoreAboutIt;
    }

    internal void GetProcessName(out string name)
    {
        name = GetProcessFilterTextInfo().m_processName;
    }

    internal void GetProcessOneLiner(out string oneLiner)
    {
        oneLiner = GetProcessFilterTextInfo().m_processOneLiner;
    }
}
