using System;
using System.Windows.Forms;

namespace Inventaria_con_listas
{
    public partial class Form1 : Form
    {
        public class Product
        {
            private int id;
            private string name;
            private double costo;

            public int Id { get { return id; } }
            public string Name { get { return name; } }
            public double Costo { get { return costo; } }

            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public Product Next { get; set; }

            public Product(int id, string name, double cost, string description, int amount)
            {
                this.id = id;
                this.name = name;
                this.costo = cost;
                this.Descripcion = description;
                this.Cantidad = amount;
            }

            public Product()
            {
                this.id = 0;
            }

            public Product(int id)
            {
                this.id = id;
            }

            public string shortDescription()
            {
                return "\n******* " + name + " *******\n" +
                        "Código: " + id + "\n";
            }

            public override string ToString()
            {
                return "\n******* " + name + " *******\n" +
                        "Código: " + id + "\n" +
                        "Cantidad: " + Cantidad + "\n" +
                        "Costo: " + costo + "\n\n" +
                        "Descripción: " + Descripcion;
            }
        }

        public class Inventory
        {
            private int length;
            public Product first;

            public string add(Product product)
            {
                if (first != null)
                {
                    Product temp = first;
                    while(temp.Next != null)
                    {
                        if (temp.Id != product.Id)
                            temp = temp.Next;
                        else
                            return "El producto con el id: " + product.Id + " ya está registrado";
                    }

                    if (temp.Id != product.Id)
                        temp.Next = product;
                    else
                        return "El producto con el id: " + product.Id + " ya está registrado";
                }
                else
                    first = product;

                return "Se agregó correctamente el producto " + product.Name;
            }

            internal string listProducts()
            {
                if (first != null)
                {
                    string productsString = first.ToString();
                    Product temp = first;
                    while(temp.Next != null)
                    {
                        temp = temp.Next;
                        productsString += temp.ToString();
                    }
                    return productsString;
                }
                else
                    return "No se ha agregado ningún producto al inventario";
            }

            internal string listByShortDescription()
            {
                if (first != null)
                {
                    string productsString = first.shortDescription();
                    Product temp = first;
                    while (temp.Next != null)
                    {
                        temp = temp.Next;
                        productsString += temp.shortDescription();
                    }
                    return productsString;
                }
                else
                    return "No se ha agregado ningún producto al inventario";
            }

            internal Product search(int id)
            {
                if(first != null)
                {
                    Product temp = first;
                    do
                    {
                        if (temp.Id == id)
                            return temp;

                        temp = temp.Next;
                    } while (temp != null);
                }

                return null;
            }

            internal void invert()
            {
                if(first != null)
                {
                    if(first.Next != null)
                    {
                        Product current = first;
                        Product before = null;
                        Product last = first.Next;

                        while(last != null)
                        {
                            current.Next = before;
                            before = current;
                            current = last;
                            last = last.Next;
                        }

                        //The last element will be in current when the loop finish
                        first = current;
                        //Is necessary assign the next to the new first product
                        first.Next = before;
                    }
                }
            }

            internal string deleteById(int id)
            {
                if (first != null)
                {
                    if (first.Id != id)
                    {
                        if(first.Next != null)
                        {
                            Product temp = first.Next;
                            Product before = first;
                            do
                            {
                                if (temp.Id != id)
                                {
                                    before = temp;
                                    temp = temp.Next;
                                }
                                else
                                {
                                    before.Next = temp.Next != null ? temp.Next : null;
                                    temp = null;
                                    return "Se eliminó correctamente el producto con el id: " + id;
                                }
                            } while (temp != null);
                        }

                        return "No se pudo eliminar el producto con id: " + id + " porque no existe";
                    }
                    else
                    {
                        first = first.Next != null ? first.Next : null;
                        return "Se eliminó correctamente el producto con el id: " + id;
                    }
                }
                else
                    return "No se ha agregado ningún producto al inventario, no se puede eliminar";
            }
        }

        Inventory inventory;

        public Form1()
        {
            InitializeComponent();
            inventory = new Inventory();
            //addProducts();
        }

        //Debuggin method
        public void addProducts()
        {
            Random a = new Random();
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine(i);
                inventory.add(new Product(a.Next(1, 8), "producto " + (i * 2), i * 2, "description " + (i * 2) + " " + (i * 2) + " " + " " + (i * 2), (i * 2)));
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            txtMostrar.Text = inventory.listByShortDescription();
            lblEstado.Text = "Elementos listados";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() != "" && txtDescripcion.Text.Trim() != "")
            {
                string agregar = inventory.add(new Product(Convert.ToInt32(numCodigo.Value), txtNombre.Text, Convert.ToDouble(numCantidad.Value), txtDescripcion.Text, Convert.ToInt32(numCantidad.Value)));
                btnListar.PerformClick();
                lblEstado.Text = agregar;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(numCodigo.Value);
            Product buscar = inventory.search(codigo);
            if (buscar != null)
            {
                lblEstado.Text = "Producto encontrado";
                MessageBox.Show(buscar.ToString());
            }
            else
            {
                lblEstado.Text = "Product no encontrado";
            }
        }

        private void btnEliminarCodigo_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(numCodigo.Value);
            string eliminar = inventory.deleteById(id);
            btnListar.PerformClick();
            lblEstado.Text = eliminar;
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            inventory.invert();
            btnListar.PerformClick();
        }
    }
}
