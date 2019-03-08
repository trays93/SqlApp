$(function() {

    addEventsForDatabaseList();
    addEventsForTableList();
    addEventsForColumnList();
    sqlKeywords();
    sqlTextarea();

    document.getElementById("sqlFile").addEventListener("change", function () {
        let formData = new FormData();
        formData.append('sqlFile', $('#sqlFile')[0].files[0]);
        let _url = '@Url.Action("Upload", "HomeController")';
        $.ajax({
            type: 'POST',
            url: '/Home/Upload',
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                console.log(result);
                let textarea = document.getElementById("sqlTextarea");
                textarea.value = result;
            }
        });
    });

});


let dragObject;

function addEventsForDatabaseList() {
    document.getElementById("databases").addEventListener("dblclick", function () {
        let x = document.getElementById("databases").value;
        let sqlTextBox = document.getElementById("sqlTextarea");
        sqlTextBox.value += "[" + x + "] ";
    });

    document.getElementById("databases").addEventListener("change", function () {
        let databaseName = document.getElementById("databases").value;
        let tables = document.getElementById("tables");

        while (tables.hasChildNodes()) {
            tables.removeChild(tables.firstChild);
        }

        let columns = document.getElementById("columns");
        while (columns.hasChildNodes()) {
            columns.removeChild(columns.firstChild);
        }

        $.ajax({
            type: 'POST',
            url: '/Home/GetTables',
            data: {
                "databaseName": databaseName
            },
            success: function (result) {
                for (let i = 0; i < result.length; i++) {
                    let element = document.createElement("option");
                    element.value = result[i];
                    let text = document.createTextNode(result[i]);
                    element.appendChild(text);
                    tables.appendChild(element);
                }
                tables.removeAttribute("disabled");
            }
        });

    });
}

function addEventsForTableList() {
    document.getElementById("tables").addEventListener("dblclick", function () {
        let x = document.getElementById("tables").value;
        let sqlTextBox = document.getElementById("sqlTextarea");
        sqlTextBox.value += "[" + x + "] ";
    });

    document.getElementById("tables").addEventListener("change", function () {
        let databaseName = document.getElementById("databases").value;
        let tableName = document.getElementById("tables").value;
        let columns = document.getElementById("columns");

        while (columns.hasChildNodes()) {
            columns.removeChild(columns.firstChild);
        }

        $.ajax({
            type: "POST",
            url: "/Home/GetColumns",
            data: {
                "databaseName": databaseName,
                "tableName": tableName
            },
            success: function (result) {
                for (let i = 0; i < result.length; i++) {
                    let element = document.createElement("option");
                    element.value = result[i];
                    let text = document.createTextNode(result[i]);
                    element.appendChild(text);
                    columns.appendChild(element);
                }
                columns.removeAttribute("disabled");
            }
        });
    });
}

function addEventsForColumnList() {
    document.getElementById("columns").addEventListener("dblclick", function () {
        let x = document.getElementById("columns").value;
        let sqlTextBox = document.getElementById("sqlTextarea");
        sqlTextBox.value += "[" + x + "] ";
    });
}

function sqlKeywords() {
  let object = document.getElementById('sqlKeywords');
  let length = object.childNodes.length;

  for (let i = 0; i < length; i++) {
    object.childNodes[i].draggable = true;
    object.childNodes[i].addEventListener("dragstart", function(ev) {
        dragObject = ev.target.textContent + " ";
      console.log(ev.target.textContent);
    });

      object.childNodes[i].addEventListener("click", function (ev) {
          let sqlTextarea = document.getElementById("sqlTextarea");
          let text = sqlTextarea.textContent;
          text += ev.target.textContent + " ";
          sqlTextarea.value += text;
      });
  }
}

function sqlTextarea() {
  let object = document.getElementById('sqlTextarea');

  object.addEventListener("drop", function(ev) {
    ev.preventDefault();
    let text = object.textContent;
    text += dragObject + " ";
      object.value = text;
  });

  object.addEventListener("dragover", function(ev) {
    ev.preventDefault();
  });
}
