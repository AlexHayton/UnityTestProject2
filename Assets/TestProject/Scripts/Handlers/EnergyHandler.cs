using UnityEngine;
using System.Collections;

public class EnergyHandler : MonoBehaviour
{

    public float maxEnergy = 100;
    public float energy = 100;

    public float regenEnergyRate = 10.0f;

    public bool infiniteEnergy = false;
    
    public EnergyHandler[] childEnergyHandlers = new EnergyHandler[] {};

    void Start()
    {
    	this.RefreshChildEnergyHandlers();
    }

    void Update()
    {
    	if (energy != maxEnergy)
    	{
        	energy = Mathf.Clamp(energy + regenEnergyRate * Time.deltaTime, 0, maxEnergy);
        }
    }
    
    void RefreshChildEnergyHandlers()
    {
    	childEnergyHandlers = transform.GetComponentsInChildren<EnergyHandler>();
    }

    public float GetEnergy()
    {
        return this.energy;
    }

    public float GetMaxEnergy()
    {
        return this.maxEnergy;
    }

    public void AddEnergy(float energy)
    {
        // not greater than maxHealth
        this.energy = Mathf.Min(this.energy + energy, maxEnergy);
    }

    public bool DeductEnergy(float energy)
    {
        float newEnergy = Mathf.Max(this.energy - energy, 0);

        if (infiniteEnergy)
        {
            newEnergy = this.energy;
        }
        else
        {
            // no negative health
            this.energy = newEnergy;
        }

        return this.energy > 0;
    }

}
