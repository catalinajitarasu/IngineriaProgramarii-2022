//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by FernFlower decompiler)
//

package com.forward_engineering;

import java.util.List;

public class Customer extends User {
    public Integer[] cuppons;
    public ReadProductsList products;
    public String ibanCustomer;
    public List<ShoppingCart> shoppingCart;

    public Customer(Integer[] cuppons, ReadProductsList products, String ibanCustomer) {
        this.cuppons = cuppons;
        this.products = products;
        this.ibanCustomer = ibanCustomer;
    }
}
