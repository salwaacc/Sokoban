mergeInto(LibraryManager.library, {
  DownloadFile: function (namePtr, dataPtr, size) {
    var name = UTF8ToString(namePtr);
    var bytes = new Uint8Array(HEAPU8.buffer, dataPtr, size);
    var blob = new Blob([bytes], { type: "image/png" });
    var url = URL.createObjectURL(blob);

    var link = document.createElement("a");
    link.download = name;
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
  }
});
