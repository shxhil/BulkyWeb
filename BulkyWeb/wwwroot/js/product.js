$(document).ready(function () {
    loadTableData()
})


function loadTableData() {
    $('#tblData').DataTable({
        "ajax": {url: '/admin/product/getall'}
    },
        "columns": [
            { data: 'Title' , "width":"15%"},
            { data: 'ISBN', "width": "15%" },
            { data: 'ListPrice', "width": "15%" },
            { data: 'Author', "width": "15%" }
            { data: 'Category.Name', "width": "15%" }
    ]
    );
}
