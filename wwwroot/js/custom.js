$(function() {

    document.getElementById("databases").addEventListener("change", function() {
        let databaseName = document.getElementById("databases").value;
        let tablesList = ["Categories", "Customers", "Employees", "Orders", "Products"];
        let tables = document.getElementById("tables");

        console.log(databaseName);

        while (tables.hasChildNodes()) {
            tables.removeChild(tables.firstChild);
        }

        $.ajax({
            type: 'POST',
            url: '/Home/GetTables',
            data: {
                "databaseName": databaseName
            },
            success: function (result) {
                console.log(databaseName);
                for (let i = 0; i < result.length; i++) {
                    console.log(result[i]);
                    let element = document.createElement("option");
                    element.value = result[i];
                    let text = document.createTextNode(result[i]);
                    element.appendChild(text);
                    tables.appendChild(element);
                }
                tables.removeAttribute("disabled");
            }
        });

        //$.post("Home/GetTables", x, function (data, status) {
        //    console.log(status);
        //    for (let i = 0; i < data.length; i++) {
        //        console.log(data[i]);
        //    }
        //});

    });

    document.getElementById("columns").addEventListener("dblclick", function () {
        let x = document.getElementById("columns").value;
        //alert(x);
        let sqlTextBox = document.getElementById("sqlTextarea");
        sqlTextBox.textContent = x;
    });

    sqlKeywords();
    sqlTextarea();


});


let dragObject;

function sqlKeywords() {
  let object = document.getElementById('sqlKeywords');
  let length = object.childNodes.length;

  for (let i = 0; i < length; i++) {
    object.childNodes[i].draggable = true;
    object.childNodes[i].addEventListener("dragstart", function(ev) {
      dragObject = ev.target.textContent;
      console.log(ev.target.textContent);
    });
  }
}

function sqlTextarea() {
  let object = document.getElementById('sqlTextarea');

  object.addEventListener("drop", function(ev) {
    ev.preventDefault();
    let text = object.textContent;
    text += dragObject + " ";
    object.textContent = text;
  });

  object.addEventListener("dragover", function(ev) {
    ev.preventDefault();
  });
}
