<template>
	<view>
		 <u-picker :show="show" ref="uPicker" :columns="columns" @confirm="confirm" @change="changeHandler"  @cancel="show=false"></u-picker>
	</view>
</template>

<script>
	const cityData = require('@/common/cityData.json')
	export default {
		data() {
			return {
				show:false,
				columns: [
					[]
					// ['中国', '美国'],
					// ['深圳', '厦门', '上海', '拉萨']
				],
				columnData: [
					// ['深圳', '厦门', '上海', '拉萨'],
					// ['得州', '华盛顿', '纽约', '阿拉斯加']
				]
			}
		},
		onLoad() {

		},
		mounted() {
			var provinces=cityData.provinces;
			provinces.forEach((value,index)=>{
				this.columns[0].push(value.provinceName);
				this.columnData.push(value.citys.map(c=>c.cityName));
			})
		},
		watch:{
		},
		methods: {
		            changeHandler(e) {
		                const {
		                    columnIndex,
		                    value,
		                    values, // values为当前变化列的数组内容
		                    index,
							// 微信小程序无法将picker实例传出来，只能通过ref操作
		                    picker = this.$refs.uPicker
		                } = e
		                // 当第一列值发生变化时，变化第二列(后一列)对应的选项
		                if (columnIndex === 0) {
		                    // picker为选择器this实例，变化第二列对应的选项
		                    picker.setColumnValues(1, this.columnData[index])
		                }
						
		            },
					// 回调参数为包含columnIndex、value、values
					confirm(e) {
		                //console.log('confirm', e)
						this.$emit("onClick",e.value[1]||e.value[0]);
		                this.show = false
					},
					open(isShow=true){
						this.show=isShow;
					},
					close(){
						this.show=false;
					}
					
				}
	}
</script>


<style>
</style>
