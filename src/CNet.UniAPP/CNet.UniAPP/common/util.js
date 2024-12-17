export const isPic=(filePath)=>{
	//获取最后一个.的位置
	let index= filePath.lastIndexOf(".");
	//获取后缀
	let ext = filePath.substr(index+1);
	const picArr=['svgz', 'pjp', 'png', 'ico', 'avif', 'tiff', 'tif', 'jfif', 'svg', 'xbm', 'pjpeg', 'webp', 'jpg', 'jpeg', 'bmp', 'gif'];
	return picArr.some(i=>i==ext);
	
}
