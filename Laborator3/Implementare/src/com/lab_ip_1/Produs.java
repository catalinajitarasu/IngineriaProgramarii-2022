package com.lab_ip_1;

public class Produs {
    private int id;
    private String nume;
    private int pret;
    private int cantitate;
    private String dataProducere;
    private String dataExpirare;

    public boolean Initialize(int id, String nume, int pret, int cantitate, String dataProducere, String dataExpirare)
    {
        if (pret < 0)
        {
            System.out.println("Invalid price");
            return false;
        }

        this.id = id;
        this.nume = nume;
        this.pret = pret;
        this.cantitate = cantitate;
        this.dataProducere = dataProducere;
        this.dataExpirare = dataExpirare;

        return true;
    }

    public int GetPretPerCantitate()
    {
        return this.pret * this.cantitate;
    }

    public void PrintDetalii()
    {
        System.out.println("< --- --- --- --- --- --- --- >");
        System.out.println("Produsul: ".concat(this.nume));
        System.out.println("Pret: ".concat(String.valueOf(this.pret)));
        System.out.println("Cantitate:  ".concat(String.valueOf(this.cantitate)));
        System.out.println("< --- --- --- --- --- --- --- >");
    }
}
