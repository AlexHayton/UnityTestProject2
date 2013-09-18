using System;

/// <summary>
/// For objects that need to check their prefabs and dependencies etc.
/// </summary>
public interface ISelfTest
{
	public bool SelfTest();
}
