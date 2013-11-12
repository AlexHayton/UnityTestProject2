// Copyright assigned 'as-is' by Jon Sagara
// See https://bitbucket.org/jonsagara/weightedrandom/wiki/Home

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides the ability to define a set of discrete, weighted items, and then to select items
/// from that set at random according to their assigned weight.
/// </summary>
/// <remarks>
/// Note that WeightedRandomItem is also defined in this file to make it easier to include this
/// logic in your projects without requiring you to link to yet another assembly.
/// </remarks>
public class WeightedRandom<TIdentifier>
{
	//
	// Constants
	//

	private const int kDefaultDistributionSize = 100;


	//
	// Instance Data and Properties
	//

	private int DistributionSize { get; set; }

	private List<WeightedRandomItem<TIdentifier>> _Items { get; set; }
	private List<TIdentifier> _WeightDistribution;

	private Random _Random = new Random();
	private object _RandomLock = new object();


	//
	// Instance Constructors
	//

	private WeightedRandom()
	{
	}

	/// <summary>
	/// Construct an instance from which you can randomly select one of the items.  An item's
	/// weight determines the frequency with which it will be returned relative to the other
	/// items.
	/// </summary>
	/// <remarks>
	/// Note that items cannot be null or empty.  Also, there can be no duplicate values 
	/// specified for WeightedRandomItem's Identifer property.
	/// </remarks>
	/// <param name="items">An array of randomly-weighted items, keyed by a user-specific identifier.  The identifiers must be unique.  By default, the weight of all items must add up to 100.</param>
	public WeightedRandom(WeightedRandomItem<TIdentifier>[] items)
		: this(items, kDefaultDistributionSize)
	{
	}

	/// <summary>
	/// Construct an instance from which you can randomly select one of the items.  An item's
	/// weight determines the frequency with which it will be returned relative to the other
	/// items.
	/// </summary>
	/// <remarks>
	/// Note that items cannot be null or empty.  Also, there can be no duplicate values 
	/// specified for WeightedRandomItem's Identifer property.
	/// </remarks>
	/// <param name="items">An array of randomly-weighted items, keyed by a user-specific identifier.  The identifiers must be unique.  The weight of all items must add up to distributionSize.</param>
	/// <param name="distributionSize">The size of the internal list used to create the weighted distribution.</param>
	private WeightedRandom(WeightedRandomItem<TIdentifier>[] items, int distributionSize)
	{
		DistributionSize = distributionSize;

		ValidateItemArray(items);
		InitializeItemLists(items);
	}


	//
	// Instance Methods -- Public Interface
	//

	/// <summary>
	/// Randomly retrieve an item.  Items with higher weights will be returned more often than 
	/// items with lower weights.
	/// </summary>
	/// <returns></returns>
	public TIdentifier Next()
	{
		int ixItem = 0;

		lock (_RandomLock)
		{
			ixItem = _Random.Next(0, _WeightDistribution.Count);
		}

		return _WeightDistribution[ixItem];
	}


	//
	// Instance Methods -- Validation and Initialization
	//

	private void ValidateItemArray(WeightedRandomItem<TIdentifier>[] items)
	{
		// Sanity checks
		if (items == null)
		{
			throw new ArgumentNullException("items", "The array of WeightedRandomItem<TIdentifer> items cannot be null");
		}

		if (items.Length == 0)
		{
			throw new ArgumentException("The array of WeightedRandomItem<TIdentifer> items cannot be empty", "items");
		}


		// Ensure we don't have any duplicate identifiers.
		HashSet<TIdentifier> ids = new HashSet<TIdentifier>();

		foreach (WeightedRandomItem<TIdentifier> item in items)
		{
			if (!ids.Contains(item.Identifier))
			{
				ids.Add(item.Identifier);
			}
			else
			{
				throw new InvalidOperationException("Duplicate identifier detected: " + item.Identifier.ToString());
			}
		}


		// Verify the weights add up to kDistributionSize.
		int weightSum = items.Sum(item => item.Weight);

		if (weightSum != DistributionSize)
		{
			throw new InvalidOperationException(
				string.Format("The weights of each item must add up to {0}.  They currently add up to {1}", DistributionSize, weightSum)
				);
		}
	}

	private void InitializeItemLists(WeightedRandomItem<TIdentifier>[] items)
	{
		// List of items.
		_Items = new List<WeightedRandomItem<TIdentifier>>(items);


		// Create the distribution of weights, a list of DistributionSize items.
		_WeightDistribution = new List<TIdentifier>(DistributionSize);

		foreach (WeightedRandomItem<TIdentifier> item in _Items)
		{
			for (int ixWeight = 0; ixWeight < item.Weight; ixWeight++)
			{
				_WeightDistribution.Add(item.Identifier);
			}
		}

		if (_WeightDistribution.Count != DistributionSize)
		{
			throw new InvalidOperationException(
				string.Format("The weight distribution list should only have a size of {0} items.  It currently contains {1}", DistributionSize, _WeightDistribution.Count)
				);
		}
	}
}


/// <summary>
/// An instance of this is used to specify a unique identifier and a weight.  A collection of
/// these items is then passed to the WeightedRandom constructor.  You use the WeightedRandom
/// instance to randomly retrieve the Identifier of one of these items.
/// </summary>
/// <typeparam name="TIdentifier">The type of &quot;key&quot; used to identify each weighted item.</typeparam>
public class WeightedRandomItem<TIdentifier>
{
	//
	// Instance Properties
	//

	/// <summary>
	/// The unique &quot;key&quot; used to identify this weighted item.
	/// </summary>
	public TIdentifier Identifier { get; private set; }

	/// <summary>
	/// The relative weight assigned to this item.
	/// </summary>
	public int Weight { get; private set; }


	//
	// Instance Constructors
	//

	private WeightedRandomItem()
	{
	}

	/// <summary>
	/// Creates an item with the speicifed unique identifier and associated weight.
	/// </summary>
	/// <param name="identifier">Uniquely identifies this item within an instance of WeightedRandom.</param>
	/// <param name="weight">The weight associated with this item.  A higher weight means it will be selected more frequently.</param>
	public WeightedRandomItem(TIdentifier identifier, int weight)
	{
		Identifier = identifier;
		Weight = weight;
	}
}