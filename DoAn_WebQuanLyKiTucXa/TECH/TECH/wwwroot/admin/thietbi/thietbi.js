(function ($) {
    var self = this;
    self.IsUpdate = false;
    self.ThietBi = {
        Id: null,
        TenThietBi: "",
        TinhTrang: "",
        GhiChu: "",
        MaLoai: ""
    };
    self.Search = {
        Keyword: "",
        maPhong: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    };

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (i + 1) + "</td>";
                html += "<td>" + item.TenThietBi + "</td>";
                html += "<td>" + item.LoaiThietBi + "</td>";
                html += "<td>" + item.TinhTrang + "</td>";
                html += "<td>" + item.GhiChu + "</td>";
                html += "<td class='text-center'>" +
                    "<button class='btn btn-warning btn-edit' data-id='" + item.Id + "'><i class='bi bi-pencil-square'></i></button> " +
                    "<button class='btn btn-danger' onclick='Deleted(" + item.Id + ")'><i class='bi bi-trash'></i></button>" +
                    "</td>";
                html += "</tr>";
            }
        } else {
            html = "<tr><td colspan='6' class='text-center'>Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };

    window.Deleted = function (id) {
        if (confirm("Bạn có chắc chắn muốn xóa?")) {
            $.ajax({
                url: '/Admin/ThietBi/Delete',
                type: 'POST',
                data: { id: id },
                success: function (res) {
                    console.log("DELETE Response:", res); // 👈 log kết quả
                    if (res && res.success) {
                        tedu.notify('Xóa thành công', 'success');
                        self.GetDataPaging(true);
                    } else {
                        tedu.notify('Xóa thất bại', 'error');
                    }
                },
                error: function (err) {
                    console.error("DELETE Error:", err); // 👈 log lỗi nếu có
                    tedu.notify('Lỗi hệ thống', 'error');
                }
            });
        }
    };

    self.GetDataPaging = function () {
        self.Search.PageIndex = tedu.configs.pageIndex;
        self.Search.PageSize = tedu.configs.pageSize;
        $.ajax({
            url: '/Admin/ThietBi/GetAllPaging',
            type: 'GET',
            data: self.Search,
            dataType: 'json',
            success: function (response) {
                self.RenderTableHtml(response.data.Results);
                $('#lblTotalRecords').text(response.data.RowCount);
                if (response.data.RowCount > 0) {
                    self.WrapPaging(response.data.RowCount, function () {
                        self.GetDataPaging();
                    });
                }
            }
        });
    };

    self.WrapPaging = function (recordCount, callBack) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        $('#paginationUL').twbsPagination('destroy');
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '<<',
            prev: '<',
            next: '>',
            last: '>>',
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack, 200);
            }
        });
    };

    self.Submit = function () {
        self.ThietBi = {
            Id: $("#Id").val(),
            TenThietBi: $("#TenThietBi").val(),
            TinhTrang: $("#TinhTrang").val(),
            GhiChu: $("#GhiChu").val(),
            MaLoai: $("#MaLoai").val()
        };

        var url = self.IsUpdate ? '/Admin/ThietBi/Update' : '/Admin/ThietBi/Add';
        $.post(url, { model: self.ThietBi }, function (res) {
            if (res.success) {
                tedu.notify('Lưu thành công', 'success');
                $('#userModal').modal('hide');
                self.GetDataPaging(true);
            } else {
                tedu.notify('Thất bại', 'error');
            }
        });
    };

    self.Validate = function () {
        $("#form-submit").validate({
            rules: {
                TenThietBi: { required: true },
                MaLoai: { required: true },
                TinhTrang: { required: true }
            },
            messages: {
                TenThietBi: { required: "Vui lòng nhập tên thiết bị" },
                MaLoai: { required: "Vui lòng chọn loại thiết bị" },
                TinhTrang: { required: "Vui lòng nhập tình trạng" }
            },
            submitHandler: function () {
                self.Submit();
            }
        });
    };

    $(document).ready(function () {
        self.GetDataPaging();
        self.Validate();

        $(".btn-addorupdate").click(function () {
            $("#form-submit")[0].reset();
            $("#Id").val(0);
            self.IsUpdate = false;
            $("#titleModal").text("Thêm mới thiết bị");
            $(".btn-submit-format").text("Thêm mới");
            $('#userModal').modal('show');
        });

        $(".btn-submit-format").click(function () {
            $("#form-submit").submit();
        });

        $('input.form-search').on('input', function () {
            self.Search.Keyword = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('.modal').on('hidden.bs.modal', function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(document).on('click', '.btn-edit', function () {
            var id = $(this).data('id');
            $.get('/Admin/ThietBi/GetById', { id: id }, function (res) {
                if (res.Data) {
                    var d = res.Data;
                    $("#Id").val(d.Id);
                    $("#TenThietBi").val(d.TenThietBi);
                    $("#TinhTrang").val(d.TinhTrang);
                    $("#GhiChu").val(d.GhiChu);
                    $("#MaLoai").val(d.MaLoai);
                    $("#titleModal").text("Cập nhật thiết bị");
                    $(".btn-submit-format").text("Cập nhật");
                    self.IsUpdate = true;
                    $('#userModal').modal('show');
                }
            });
        });
    });
})(jQuery);
