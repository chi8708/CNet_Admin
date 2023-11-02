<template>
    <div>
        哈哈
        <Button type="primary" @click="wordView">word预览</Button>
    </div>
</template>

<script>
// import pdfview from './pdfview.vue';

export default {
    components: {
        //pdfview
    },
    name: '',
    data() {
        return {

        }
    },
    methods: {
        wordView() {
            alert("111");
            this.openNewWindow();
        },
        openNewWindow() {
            const { href } = this.$router.resolve({
                //直接访问word
                path: "/demo/wordview/?filePath=http://localhost:10910/api/WikiMain/GetDocxFile?path=/FileUpload/wiki/1.docx",
            });
            window.open(href, "_blank");//, "width=900, height=700"
        },
    }
}
</script>

<style lang="" scoped>
    
</style>