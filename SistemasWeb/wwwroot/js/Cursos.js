﻿

class Cursos extends Uploadpicture {

    constructor() {
        super();
        this.Image = null;
        this.CursoID = 0;
    }

    RegistrarCurso() {
        var data = new FormData();
        data.append('Input.Nombre', $("#curNombre").val());
        data.append('Input.Descripcion', $("#curDescripcion").val());
        data.append('Input.Horas', $("#curHoras").val());
        data.append('Input.Costo', $("#curCosto").val());
        data.append('Input.Estado', document.getElementById("curEstado").checked);
        data.append('Input.CategoriaID', $("#curCatergoria").val());

        data.append('Input.Image', this.Image);
        data.append('Input.CursoID', $("#curCursoID").val());

        $.each($('input[type=file]')[0].files, (i, file) => {
            data.append('AvatarImage', file);
        });

        $.ajax({
            url: "GetCurso",
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: (result) => {
                try {
                    var item = JSON.parse(result);
                    if (item.Code == "Done") {
                        window.location.href = "Cursos";
                    } else {
                        document.getElementById("mensaje").innerHTML = item.Description;
                    }
                } catch (e) {
                    document.getElementById("mensaje").innerHTML = result;
                }
                console.log(result);
            }
        });
    }

    EditCurso(curso, cat) {
        document.getElementById("myModalLabel").innerHTML = "Editar Curso";

        let j = 1;
        $("#curNombre").val(curso.Nombre);
        $("#curDescripcion").val(curso.Descripcion);
        $("#curHoras").val(curso.Horas);
        $("#curCosto").val(curso.Costo);
        $("#curEstado").prop("checked", curso.Estado);
        $("#curCursoID").val(curso.CursoID);
        this.Image = curso.Image;

        document.getElementById("cursoImage").innerHTML = "<img class='cursoImage' src='data:image/jpeg;base64," + curso.Image + "' />";
        let x = document.getElementById("curCatergoria");
        x.options.length = 0;

        for (var i = 0; i < cat.length; i++) {
            if (cat[i].Value == curso.CategoriaID) {

                x.options[0] = new Option(cat[i].Text, cat[i].Value);
                x.selectedIndex = 0;
                j = i;
            } else {

                x.options[i] = new Option(cat[i].Text, cat[i].Value);
            }

        }

        if (1 < curso.CategoriaID) {
            x.options[j] = new Option(cat[0].Text, cat[0].Value);
        }

        console.log(curso);
        console.log(cat);
    }

    limpiar() {
        document.getElementById("myModalLabel").innerHTML = "Agregar Curso";
        $("#curNombre").val("");
        $("#curDescripcion").val("");
        $("#curHoras").val(0);
        $("#curCosto").val(0);
        $("#curEstado").prop("checked", true);
        $("#curCursoID").val("");

        document.getElementById("cursoImage").innerHTML = ['<img class="cursoImage" src="', "/Images/logo-google.png", '"/>'].join('');

    }

}
