package com.lab_ip_1;

public class Main {

    public static void main(String[] args) {
	    Main mainObj = new Main();
        mainObj.cerinta();
        //mainObj.tratezInvalid();
    }

    public void cerinta()
    {
        Cos cosCumparaturi = new Cos(100);

        Produs ciocolata = new Produs();
        ciocolata.Initialize(1, "Primola", 3, 5, "5-02-2020",
                "10-01-2021");

        Produs trandafir = new Produs();
        trandafir.Initialize(1, "Trandafir", 15, 2, "5-02-2020",
                "10-01-2021");

        Produs esarfa = new Produs();
        esarfa.Initialize(1, "Esarfa", 60, 1, "-","-");

        cosCumparaturi.AddProdus(ciocolata);
        cosCumparaturi.AddProdus(trandafir);
        cosCumparaturi.AddProdus(esarfa);

        System.out.println("Costul produselor este : ".concat( String.valueOf(cosCumparaturi.GetTotalCost())) );

        for (int it = 0; it < cosCumparaturi.GetNrOfProducts(); it++)
        {
            Produs produsCurent = cosCumparaturi.GetProdus(it);

            produsCurent.PrintDetalii();
        }
    }

    public void tratezInvalid()
    {
        Produs ciocolata = new Produs();
        ciocolata.Initialize(1, "Primola", -3, 5, "5-02-2020",
                "10-01-2021");
    }
}
