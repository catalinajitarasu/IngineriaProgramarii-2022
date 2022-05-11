package com.forward_engineering;

import java.util.List;

public class ShoppingCart {

  private Integer maxProducts;

  public PaymentSystem PaymentSystem;

    public List<ProductCollection> productCollection;

  public ShoppingCart(Integer maxProducts, com.forward_engineering.PaymentSystem paymentSystem) {
    this.maxProducts = maxProducts;
    PaymentSystem = paymentSystem;
  }

  public void addProduct(Product product) {
  }

  public void deletePorduct(Integer id) {
  }

  public void increeseQuantity(Integer id) {
  }

  public void submitProducts() {
  }

}