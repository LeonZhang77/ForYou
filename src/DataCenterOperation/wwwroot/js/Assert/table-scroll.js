$(function() {
            $('.table-scroll').scroll(function() {
                  $('.table-scroll table').width($('.table-scroll').width() 
                  + $('.table-scroll').scrollLeft());
                });

                var tableTdWidths = new Array();
            var tableWidth = 0;
            var tableTr0Width = 0;
            var tableThNum = 0;
            var tableTr1Width = 0;

                tableWidth = $('.table-scroll table').css('width').replace('px','');
                tableThNum = $('.table-scroll tr:eq(0)').children('th').length;

            if ($('.table-scroll tr').length == 1) { // header only
                if (tableWidth > tableTr0Width) {
                    $('.table-scroll tr:eq(0)').children('th').each(function(i){
                        $(this).width(parseInt(($(this).css('width').replace('px','')) 
                        + parseInt(Math.floor((tableWidth - tableTr0Width) / tableThNum))) + 'px');
                    });
                }
            } else { // header and body
                tableTr1Width = $('.table-scroll tr:eq(1)').css('width').replace('px','');
                    $('.table-scroll tr:eq(1)').children('td').each(function(i){
                    tableTdWidths[i]=$(this).css('width').replace('px','');
                });
                $('.table-scroll tr:eq(0)').children('th').each(function(i) {
            if(parseInt($(this).css('width').replace('px', '')) >
                parseInt(tableTdWidths[i])) {
                tableTdWidths[i] = $(this).css('width').replace('px','');
                    }
                });

                if (tableWidth > tableTr1Width) {
                    //set all th td width
                    $('.table-scroll tr').each(function(i){
                            $(this).children().each(function(j){
                                $(this).css('min-width',(parseInt(tableTdWidths[j]) 
                                + parseInt(Math.floor((tableWidth - tableTr1Width) / 
                                tableThNum))) + 'px');
                            });
                    });
                } else {
                    //method 1 : set all th td width
                    $('.table-scroll tr').each(function(i){
                            $(this).children().each(function(j){
                                $(this).css('min-width',tableTdWidths[j] + 'px');
                            });
                    });
                }
            }
            }); 

