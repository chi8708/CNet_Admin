

    <!-- 此代码由CNetCodeGen生成， 作者：cts 生成时间：2025-06-30 17:01:14 -->

<template>
    <div class="my-layout">
        <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
            <el-form :inline="true" @submit.stop.prevent>
                    <el-form-item label="">
                        <el-input v-model="queryData.SL_Id" placeholder="" @keyup.enter="onQuery" />
                    </el-form-item>
                    <el-form-item label="">
                        <el-input v-model="queryData.SL_Name" placeholder="" @keyup.enter="onQuery" />
                    </el-form-item>
                <el-form-item>
                    <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
                </el-form-item>
            </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
            <el-row class="mb-4">
                <el-button v-auth="access.REMOVE" type="primary" icon="ele-Plus" @click="onAdd" size="small">
                    新增
                </el-button>
            </el-row>
            <el-table :data="state.pageList" style="width: 100%" v-loading="state.loading" row-key="Id" border stripe>
                    <el-table-column prop="Id" label="" min-width="80" show-overflow-tooltip />
                    <el-table-column prop="Name" label="" min-width="80" show-overflow-tooltip />
                <el-table-column label="操作" width="200" fixed="right" header-align="center" align="center">
                    <template #default="{ row }">
                        <el-button v-if="auth(access.EDIT) && row.parentCode != 0" icon="ele-EditPen" size="small"
                                   text type="primary" @click="onEdit(row)">编辑</el-button>
                        <el-button v-if="auth(access.REMOVE) && row.parentCode != 0" icon="ele-Delete" size="small"
                                   text type="danger" @click="onDelete(row)">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <div class="my-flex my-flex-end" style="margin-top: 20px">
                <el-pagination v-model:currentPage="state.pageInput.pageNum" v-model:page-size="state.pageInput.pageSize"
                               :total="state.total" :page-sizes="[10, 20, 50, 100]" small background @size-change="onSizeChange"
                               @current-change="onCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
            </div>
        </el-card>

        <edit ref="editRef" :title="state.edit.title" @on-save-success="onQuery"></edit>
    </div>
</template>

<script lang="ts" setup name="/baseSet/Role">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { test_table } from '/@/api/model/test_table'
import { test_tableApi } from '/@/api/admin/test_table'
import eventBus from '/@/utils/mitt'
import { test_tableas access } from '/@/access/test_table'

// 引入组件
const edit = defineAsyncComponent(() => import('./edit.vue'))


const { proxy } = getCurrentInstance() as any

const editRef = ref()

const state = reactive({
    loading: false,
    pageInput: {
        pageNum: 1,
        pageSize: 10,
        field: 'Id',
        order: 'desc',
        query: {} as any,
    },
    pageList: [] as Array<test_table>,
    total: 0,
    edit: {
        title: '新增'
    },
})

onMounted(() => {
    onQuery()
    eventBus.off('refreshList')
    eventBus.on('refreshList', async () => {
        onQuery()
    })
})

onBeforeMount(() => {
   eventBus.off('refreshList')
})

const onQuery = async () => {
    state.loading = true
        const res = await new test_tableApi().getPage_base <test_table> (state.pageInput).catch(() => {
        state.loading = false
    })

    state.pageList = res?.data ?? []
    state.total = res?.count ?? 0
    state.loading = false
}
const onSizeChange = (val: number) => {
    state.pageInput.pageSize = val
    onQuery()
}

const onCurrentChange = (val: number) => {
    state.pageInput.pageNum = val
    onQuery()
}

const onAdd = () => {
    state.edit.title = '新增'
    editRef.value.open()
}

 const onEdit = (row: test_table) => {
    state.edit.title = '编辑'
    editRef.value.open(row)
}

  const onDelete = (row: test_table) => {
    proxy.$modal
        .confirmDelete(`确定要删除【${row.Id}】?`)
        .then(async () => {
            await new test_tableApi().delete_base(row.Id, { loading: true })
            onQuery()
        })
        .catch(() => { })
}


</script>

<style scoped lang="scss"></style>

