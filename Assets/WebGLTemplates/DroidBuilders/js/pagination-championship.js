$(document).ready(function(){
	var totalPage = parseInt($('#totalPagesChampionship').val());
	console.log("==totalPage=="+totalPage);
	var pag = $('#pagination-championship').simplePaginator({
		totalPages: totalPage,
		maxButtonsVisible: 5,
		currentPage: 1,
		nextLabel: 'Next',
		prevLabel: 'Prev',
		firstLabel: 'First',
		lastLabel: 'Last',
		clickCurrentPage: true,
		pageChange: function(page) {
			$("#championship-content").html('<tr><td colspan="6"><strong>loading...</strong></td></tr>');
            $.ajax({
				url:"get_championship.php",
				method:"POST",
				dataType: "json",
				data:{page:	page},
				success:function(responseData){
					$('#championship-content').html(responseData.html);
				}
			});
		}
	});
});
