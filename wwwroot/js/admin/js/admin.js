$(document).ready(function () {
	"use strict"; // start of use strict

	/*==============================
	Menu
	==============================*/
	$('.header__btn').on('click', function() {
		$(this).toggleClass('header__btn--active');
		$('.header').toggleClass('header--active');
		$('.sidebar').toggleClass('sidebar--active');
	});

	/*==============================
	Filter
	==============================*/
	$('.filter__item-menu li').each( function() {
		$(this).attr('data-value', $(this).text().toLowerCase());
	});

	$('.filter__item-menu li').on('click', function() {
		var text = $(this).text();
		var item = $(this);
		var id = item.closest('.filter').attr('id');
		$('#'+id).find('.filter__item-btn input').val(text);
	});

	/*==============================
	Tabs
	==============================*/
	$('.profile__mobile-tabs-menu li').each( function() {
		$(this).attr('data-value', $(this).text().toLowerCase());
	});

	$('.profile__mobile-tabs-menu li').on('click', function() {
		var text = $(this).text();
		var item = $(this);
		var id = item.closest('.profile__mobile-tabs').attr('id');
		$('#'+id).find('.profile__mobile-tabs-btn input').val(text);
	});

	/*==============================
	Modal
	==============================*/
	$('.open-modal').magnificPopup({
		fixedContentPos: true,
		fixedBgPos: true,
		overflowY: 'auto',
		type: 'inline',
		preloader: false,
		focus: '#username',
		modal: false,
		removalDelay: 300,
		mainClass: 'my-mfp-zoom-in',
	});

	$('.modal__btn--dismiss').on('click', function (e) {
		e.preventDefault();
		$.magnificPopup.close();
	});

	/*==============================
	Select2
	==============================*/
	if ($('#country').length) {
		$('#country').select2({
			placeholder: "Chọn quốc gia"
		});
	}
	if ($('#category').length) {
		$('#category').select2({
			placeholder: "Chọn thể loại",
			multiple: true
		});
	}
	if ($('#rated').length) {
		$('#rated').select2({
			placeholder: "Choose rated"
		});
	}
	if ($('#genre').length) {
		$('#genre').select2({
			placeholder: "Choose genre / genres"
		});
	}

	// Initialize other selects only if they exist
	$('#cinema, #room, #seat_type').each(function() {
		if ($(this).length) {
			$(this).select2();
		}
	});

	/*==============================
	Upload cover
	==============================*/
	function readURL(input) {
		if (input.files && input.files[0]) {
			var reader = new FileReader();

			reader.onload = function(e) {
				$('#form__img').attr('src', e.target.result);
			}
		
			reader.readAsDataURL(input.files[0]);
		}
	}

	$('.form__img-upload').on('change', function() {
		readURL(this);
	});

	/*==============================
	Upload video
	==============================*/
	$('.form__video-upload').on('change', function() {
		var videoLabel  = $(this).attr('data-name');

		if ($(this).val() != '') {
			$(videoLabel).text($(this)[0].files[0].name);
		} else {
			$(videoLabel).text('Upload video');
		}
	});

	/*==============================
	Upload gallery
	==============================*/
	$('.form__gallery-upload').on('change', function() {
		var length = $(this).get(0).files.length;
		var galleryLabel  = $(this).attr('data-name');

		if( length > 1 ){
			$(galleryLabel).text(length + " files selected");
		} else {
			$(galleryLabel).text($(this)[0].files[0].name);
		}
	});

	/*==============================
	Scroll bar
	==============================*/
	$('.scrollbar-dropdown').mCustomScrollbar({
		axis: "y",
		scrollbarPosition: "outside",
		theme: "custom-bar"
	});

	$('.main__table-wrap').mCustomScrollbar({
		axis: "x",
		scrollbarPosition: "outside",
		theme: "custom-bar2",
		advanced: {
			autoExpandHorizontalScroll: true
		}
	});

	$('.dashbox__table-wrap').mCustomScrollbar({
		axis: "x",
		scrollbarPosition: "outside",
		theme: "custom-bar3",
		advanced: {
			autoExpandHorizontalScroll: 2
		}
	});

	/*==============================
	Bg
	==============================*/
	$('.section--bg').each( function() {
		if ($(this).attr("data-bg")){
			$(this).css({
				'background': 'url(' + $(this).data('bg') + ')',
				'background-position': 'center center',
				'background-repeat': 'no-repeat',
				'background-size': 'cover'
			});
		}
	});

});