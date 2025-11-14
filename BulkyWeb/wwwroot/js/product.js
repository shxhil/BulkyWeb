
$(document).ready(function () {
    loadTableData();
});

function loadTableData() {
    new DataTable('#tblData', {
        "ajax": {
            "url": '/Admin/Product/GetAll',
            "dataSrc": 'data' // This matches the { data = productObj } from your controller
        },
        "columns": [
            { data: 'title', width: "15%" },
            { data: 'isbn', width: "15%" },
            { data: 'listPrice', "render": data => `$${data.toFixed(2)}`, width: "10%" }, // Formats price
            { data: 'author', width: "15%" },
            { data: 'category.name', width: "15%" },
            {
                // This is the 6th column for the buttons
                data: 'id', // Use the 'id' of the product for the links
                "render": function (data) {
                    // 'data' is the product's ID
                    return `
                        <div role="group" class="btn-group w-100">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="/Admin/Product/Delete?id=${data}" class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                    `;
                },
                width: "20%"
            }
        ]
    });
}