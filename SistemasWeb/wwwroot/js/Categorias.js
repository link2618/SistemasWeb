
class Categorias {
    constructor() {
        this.CategoriaID = 0;
    }

    RegistrarCategoria() 
    {
        $.post(
            "GetCategorias",
            $('.formCategoria').serialize(),
            (response) => {
                try {
                    var item = JSON.parse(response);

                    if (item.Code == "Done") {
                        window.location.href = "Categoria";
                    } else {
                        document.getElementById("mensaje").innerHTML = item.Description;
                    }
                } catch (e) {
                    document.getElementById("mensaje").innerHTML = response;
                }

                console.log(response);
            }
        );
    }

    EditCategoria(data)
    {
        document.getElementById("myModalLabel").innerHTML = "Editar categoria";
        document.getElementById("catNombre").value = data.Nombre;
        document.getElementById("catDescripcion").value = data.Descripcion;
        document.getElementById("catEstado").checked = data.Estado; 
        document.getElementById("catCategoriaID").value = data.CategoriaID;
        console.log(data);
    }

    limpiar() {
        document.getElementById("myModalLabel").innerHTML = "Agregar categoria";
        document.getElementById("catNombre").value = "";
        document.getElementById("catDescripcion").value = "";
        document.getElementById("catEstado").checked = true;
        document.getElementById("catCategoriaID").value(0);
    }

    GetCategoria(data)
    {
        document.getElementById("titleCategoria").innerHTML = data.Nombre;
        this.CategoriaID = data.CategoriaID;
    }

    EliminarCategoria()
    {
        $.post(
            "EliminarCategoria",
            { CategoriaID: this.CategoriaID },
            (response) => {
                var item = JSON.parse(response);

                if (item.Description == "Done") {
                    window.location.href = "Categoria";
                } else {
                    document.getElementById("mensajeEliminar").innerHTML = item.Description;
                }
            }
        );
    }
}
