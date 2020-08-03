$(document).ready(function(){
	var totalPage = parseInt($('#totalPagesFastest').val());
	console.log("==totalPage=="+totalPage);
	var pag = $('#pagination-fastest').simplePaginator({
		totalPages: totalPage,
		maxButtonsVisible: 5,
		currentPage: 1,
		nextLabel: 'Next',
		prevLabel: 'Prev',
		firstLabel: 'First',
		lastLabel: 'Last',
		clickCurrentPage: true,
		pageChange: function(page) {
			$("#fastest-content").html('<tr><td colspan="6"><strong>loading...</strong></td></tr>');
            $.ajax({
				url:"get_fastest.php",
				method:"POST",
				dataType: "json",
				data:{page:	page},
				success:function(responseData){
					$('#fastest-content').html(responseData.html);
				}
			});
		}
	});
});
