package com.lab_ip_1;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class Cos {
    private List<Produs> produseCos;
    private int maxProduseCos;

    public Cos(int maxProduseCos)
    {
        this.maxProduseCos = maxProduseCos;
        this.produseCos = new ArrayList<Produs>();
    }

    public void AddProdus(Produs produs)
    {
        if (this.produseCos.size() < this.maxProduseCos)
        {
            this.produseCos.add(produs);
        }
    }

    public Produs GetProdus(int index)
    {
        return this.produseCos.get(index);
    }

    public int GetNrOfProducts()
    {
        return this.produseCos.size();
    }

    public int GetTotalCost()
    {
        int currentCost = 0;

        for (Produs produsCurent : this.produseCos)
        {
           currentCost += produsCurent.GetPretPerCantitate();
        }

        return currentCost;
    }
}
