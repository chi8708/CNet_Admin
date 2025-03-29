<template>
  <div class="container">
    <el-card class="upload-card">
      <template #header>
        <div class="card-header">
          <h2>Swagger to Vue API Generator</h2>
        </div>
      </template>
      
      <el-form :model="form" label-width="120px">
        <el-form-item label="Swagger URL">
          <el-input v-model="form.swaggerUrl" placeholder="输入Swagger JSON URL" />
        </el-form-item>
        
        <el-form-item label="Swagger文件">
          <el-upload
            class="upload-demo"
            drag
            action=""
            :auto-upload="false"
            :on-change="handleFileChange"
            accept=".json">
            <el-icon class="el-icon--upload"><upload-filled /></el-icon>
            <div class="el-upload__text">
              拖拽文件到此处或 <em>点击上传</em>
            </div>
          </el-upload>
        </el-form-item>
        
        <el-form-item>
          <el-button type="primary" @click="handleGenerate" :loading="loading">
            生成API代码
          </el-button>
        </el-form-item>
      </el-form>

      <div v-if="generatedFiles.length > 0" class="generated-files">
        <div class="files-header">
          <h3>生成的文件：</h3>
          <el-button 
            type="primary" 
            @click="downloadSelected" 
            :disabled="!selectedFiles.length"
          >
            下载选中文件
          </el-button>
        </div>
        
        <div class="search-box">
          <el-input
            v-model="searchQuery"
            placeholder="搜索文件名或路径"
            prefix-icon="Search"
            clearable
            @clear="handleSearchClear"
            style="margin-bottom: 16px;"
          />
        </div>

        <el-table 
          :data="paginatedData"
          style="width: 100%"
          v-loading="loading"
          @selection-change="handleSelectionChange"
        >
          <el-table-column type="selection" width="55" />
          <el-table-column prop="fileName" label="文件名" />
          <el-table-column prop="paths" label="路径">
            <template #default="scope">
              <div v-for="(path, index) in scope.row.paths" :key="index" class="api-path">
                <span class="path-text">{{ path.path }}</span>
              </div>
            </template>
          </el-table-column>
          <el-table-column label="操作" width="120" fixed="right">
            <template #default="scope">
              <el-button 
                @click="viewContent(scope.row)" 
                size="small" 
                type="primary"
              >
                查看代码
              </el-button>
            </template>
          </el-table-column>
        </el-table>

        <div class="pagination-container">
          <el-pagination
            v-model:current-page="currentPage"
            v-model:page-size="pageSize"
            :page-sizes="[10, 20, 50, 100]"
            :total="filteredData.length"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
            layout="total, sizes, prev, pager, next, jumper"
          >
            <template #total>
              总计 {{ filteredData.length }} 条
            </template>
            <template #sizes>
              <el-select v-model="pageSize" style="width: 110px">
                <el-option
                  v-for="size in [10, 20, 50, 100]"
                  :key="size"
                  :value="size"
                  :label="`${size}条/页`"
                />
              </el-select>
            </template>
            <template #prev>
              <span>上一页</span>
            </template>
            <template #next>
              <span>下一页</span>
            </template>
          </el-pagination>
        </div>
      </div>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      title="文件内容"
      width="60%"
      :close-on-click-modal="false"
    >
      <div class="code-content">
        <div class="code-header">
          <el-button 
            type="primary" 
            size="small" 
            @click="copyCode"
            :icon="Document"
          >
            复制代码
          </el-button>
        </div>
        <pre v-if="selectedFileContent" class="code-block">{{ selectedFileContent }}</pre>
        <el-empty v-else description="没有代码内容" />
      </div>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, computed } from 'vue'
import { ElMessage } from 'element-plus'
import { UploadFilled, Document, Search } from '@element-plus/icons-vue'
import request from '../utils/request'

const form = reactive({
  swaggerUrl: ''
})

const loading = ref(false)
const dialogVisible = ref(false)
const selectedFileContent = ref('')
const generatedFiles = ref([])
const searchQuery = ref('')
const currentPage = ref(1)
const pageSize = ref(20)

const filteredData = computed(() => {
  if (!searchQuery.value) {
    return generatedFiles.value
  }
  const query = searchQuery.value.toLowerCase()
  return generatedFiles.value.filter(file => 
    file.fileName.toLowerCase().includes(query) ||
    file.paths.some(p => p.path.toLowerCase().includes(query))
  )
})

const paginatedData = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filteredData.value.slice(start, end)
})

const handleSearchClear = () => {
  searchQuery.value = ''
}

const handleSizeChange = (val) => {
  pageSize.value = val
  currentPage.value = 1
}

const handleCurrentChange = (val) => {
  currentPage.value = val
}

const swaggerFile = ref(null)

const handleFileChange = (file) => {
  swaggerFile.value = file.raw
}

const handleGenerate = async () => {
  if (!form.swaggerUrl && !swaggerFile.value) {
    ElMessage.error('请提供Swagger URL或上传Swagger文件')
    return
  }

  loading.value = true
  try {
    const formData = new FormData()
    if (form.swaggerUrl) {
      formData.append('swaggerUrl', form.swaggerUrl)
    }
    if (swaggerFile.value) {
      formData.append('swaggerFile', swaggerFile.value)
    }

    const result = await request({
      url: '/api/swagger/generate',
      method: 'post',
      data: formData,
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })

    if (result.success) {
      generatedFiles.value = result.groups.map(group => ({
        fileName: group.name,
        paths: group.files.map(file => ({
          path: file.path,
          content: file.content
        }))
      }))
      currentPage.value = 1
      ElMessage.success('API代码生成成功！')
    } else {
      ElMessage.error(result.message || '生成失败')
    }
  } catch (error) {
    console.error('Error:', error)
    ElMessage.error('生成过程中发生错误')
  } finally {
    loading.value = false
  }
}

const viewContent = (row) => {
  if (row.paths && row.paths.length > 0) {
    selectedFileContent.value = row.paths[0].content
    dialogVisible.value = true
  } else {
    ElMessage.warning('没有可显示的代码内容')
  }
}

const copyCode = async () => {
  try {
    await navigator.clipboard.writeText(selectedFileContent.value)
    ElMessage({
      message: '代码已复制到剪贴板',
      type: 'success',
      duration: 2000
    })
  } catch (err) {
    ElMessage({
      message: '复制失败，请手动复制',
      type: 'error',
      duration: 2000
    })
    console.error('复制失败:', err)
  }
}

const selectedFiles = ref([])

const handleSelectionChange = (selection) => {
  selectedFiles.value = selection
}

const downloadSelected = () => {
  if (selectedFiles.value.length === 0) {
    ElMessage.warning('请选择要下载的文件')
    return
  }

  selectedFiles.value.forEach(file => {
    if (file.paths && file.paths.length > 0) {
      const content = file.paths[0].content
      const fileName = file.fileName.endsWith('.js') ? file.fileName : `${file.fileName}.js`
      
      const blob = new Blob([content], { type: 'text/javascript' })
      const link = document.createElement('a')
      link.href = URL.createObjectURL(blob)
      link.download = fileName
      document.body.appendChild(link)
      link.click()
      document.body.removeChild(link)
      URL.revokeObjectURL(link.href)
    }
  })

  ElMessage.success('文件下载成功')
}
</script>

<style scoped>
.container {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.upload-card {
  margin-top: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.generated-files {
  margin-top: 20px;
}

.code-content {
  position: relative;
}

.code-header {
  margin-bottom: 10px;
  display: flex;
  justify-content: flex-end;
}

.code-block {
  white-space: pre-wrap;
  word-wrap: break-word;
  background-color: #f5f5f5;
  padding: 15px;
  border-radius: 4px;
  margin: 0;
  max-height: 600px;
  overflow-y: auto;
  font-family: monospace;
  font-size: 14px;
  line-height: 1.5;
}

:deep(.el-dialog__body) {
  padding: 20px;
}

.api-path {
  margin-bottom: 4px;
}

.path-text {
  font-family: monospace;
}

.pagination-container {
  margin-top: 16px;
  display: flex;
  justify-content: flex-end;
}

.search-box {
  margin-bottom: 16px;
}

.files-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}

.files-header h3 {
  margin: 0;
}
</style> 