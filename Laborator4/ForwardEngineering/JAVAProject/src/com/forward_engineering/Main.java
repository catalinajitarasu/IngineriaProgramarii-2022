package com.forward_engineering;

public class Main {

    public static void main(String[] args) {
	    AllCrudProductsList productsList = new AllCrudProductsList();
        ReadProductsList readProductList = new ReadProductsList(productsList);
    }
}
