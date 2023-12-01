
$(document).ready(function () {
    $('.js-delete').on('click', function (e) {
        var btn = $(this);
        const swal = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger mx-2",
                cancelButton: "btn btn-light"
            },
            buttonsStyling: false
        });
        swal.fire({
            title: "Are you sure that you need to delete this game?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/Games/Delete/${btn.data('id')}`,
                    method: 'DELETE',
                    success: function () {
                        swal.fire({
                            title: "Deleted!",
                            text: "Game has been deleted.",
                            icon: "success"
                        });
                        btn.parents('tr').fadeOut();
                    },
                    error: function (err) {
                        swal.fire({
                            title: "Oooops...",
                            text: "Something went wrong.",
                            icon: "error"
                        });
                    }

                });
                
            }
        });
        
    });
});
