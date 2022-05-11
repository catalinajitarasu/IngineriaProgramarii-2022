#include <iostream>
#include <cstring>
using namespace std;


class Product
{
protected:
    char *name;
    int price;
    int quantity;

public:
    virtual void Get_info() = 0;

    int Get_quantity()
    {
        return quantity;
    }

    void Increment_quantity()
    {
        quantity++;
    }

    void Decrement_quantity()
    {
        quantity--;
    }

};

class Carte : public Product
{
    char *type;
    int no_of_pages;
public:
    Carte()
    {
        quantity = 0;
    }
    
    Carte(const char *Name, int Price, const char Type[], int No_of_pages)
    {
        name = new char(strlen(Name)+1);
        strcpy_s(name, strlen(Name), Name);
        price = Price;
        type = new char(strlen(Type) + 1);
        strcpy_s(type, strlen(Type), Type);
        no_of_pages = No_of_pages;
        quantity = 1;
    }

    void Get_info()
    {
        cout << name << ", " << type << ", " << no_of_pages << ", " << price << "\n";
    }

};

class Jucarii : public Product
{
    int age;
    char gender[100];
    Jucarii()
    {
        quantity = 0;
    }

    void Get_info()
    {
        cout << name << ", " << age << ", " << gender << ", " << price << "\n";
    }
};

class Fashion : public Product
{
    char brand[100];
    char size[4];
    char color[100];
    char gender[100];

    void Get_info()
    {
        cout << name << ", " << brand << ", " << size << ", " << color << ", " << gender << ", " << price << "\n";
    }
};

class Shopping_cart
{
    int count_different_products;
    Product *products;

public:
    void Add_product();
    void Delete_product();
    void Compute_final_sum();

};


int main()
{
    Carte c();
}

