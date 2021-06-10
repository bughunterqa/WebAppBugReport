$('.open-details-modal').on('click', function (event) {
    var modal = $(event.target).siblings('.details-modal');
    modal.modal('show');

   /* var button = $(event.target);
    var recipient = button.data('whatever');

    modal.find('.modal-title').text('New message to ' + recipient);
    modal.find('.modal-body input').val(recipient);*/

});

